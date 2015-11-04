namespace PhotoContest.Web.Controllers
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.WindowsAzure.Storage.Blob;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;
    using PhotoContest.Data;
    using PhotoContest.Models;
    using PhotoContest.Web.Models.BindingModels;
    using PhotoContest.Web.ViewModels;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using PagedList;
    using System.Net;

    public class ContestController : BaseController
    {
        public ContestController(IPhotoContestData data)
            : base(data)
        {
        }


        public ActionResult PastContests(int? page)
        {
            var inactiveContests = this.Data.Contests
                .All()
                .OrderBy(x => x.StartDate)
                .Project().To<PastContestViewModel>()
                .Where(c => c.Flag.Equals("Inactive"))
                .ToPagedList(page ?? 1, 3);

            return this.View(inactiveContests);

        }


        public ActionResult Create()
        {
            this.LoadCategories();

            return this.View();
        }

        //GET Contest
        public ActionResult Details(int id)
        {
            //New Version
            var contest = this.Data.Contests
                                .All()
                                .FirstOrDefault(c => c.Id == id);

            var contestViewModel = Mapper.Map<ContestDetailsViewModel>(contest);

            contestViewModel.CreatorName = contest.Creator.UserName;

            contestViewModel.Participants = this.Data.Images
                                                    .All()
                                                    .Where(img => img.ContestId == id)
                                                    .Select(u => u.Author.Id)
                                                    .Distinct()
                                                    .Count();

            var numberOfPrizesInDB = this.Data.Prizes.All().Where(i => i.ContestId == contestViewModel.Id).Count();
            this.ViewBag.NumOfPrizesInDB = numberOfPrizesInDB;
            this.ViewBag.NumOfWinners = contestViewModel.NumberOfPrizes;

            return this.View(contestViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContestBindingModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                if (model != null && this.ModelState.IsValid)
                {
                    var newContest = Mapper.Map<Contest>(model);
                    newContest.StartDate = DateTime.Now;
                    newContest.CreatorId = this.User.Identity.GetUserId();
                    if (model.RewardStrategy == RewardStrategy.One)
                    {
                        newContest.NumberOfPrizes = 1;
                    }

                    this.Data.Contests.Add(newContest);
                    this.Data.SaveChanges();

                    return this.RedirectToAction("Details", "Contest", new { newContest.Id });
                }
            }

            this.LoadCategories();

            return this.View(model);
        }

        //new version
        [HttpGet]
        public ActionResult CreatePrizes(int contestId, int numOfWinnersRequired, int leftForAdding)
        {
            this.ViewBag.leftForAdding = leftForAdding;
            this.ViewBag.ContestId = contestId;
            this.ViewBag.numOfWinnersRequired = numOfWinnersRequired;
            return this.View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreatePrizes(CreatePrizesBindingModel prizesModel, int contestId, int numOfWinnersRequired, int leftForAdding)
        {
            if (prizesModel != null && this.ModelState.IsValid)
            {
                var prize = new Prize()
                {
                    ContestId = contestId,
                    PrizeName = prizesModel.PrizeName,
                    Position = leftForAdding
                };
                this.Data.Prizes.Add(prize);
                this.Data.SaveChanges();
            }
            int numOfWinnersInDb = this.Data.Prizes.All().Where(p => p.ContestId == contestId).Count();

            if (numOfWinnersInDb < numOfWinnersRequired)
            {
                //If all winners prizes are'nt added to DB
                leftForAdding = numOfWinnersRequired - numOfWinnersInDb;
                this.ViewBag.leftForAdding = leftForAdding;
                return this.RedirectToAction("CreatePrizes", "Contest", new { contestId = contestId, numOfWinnersRequired = numOfWinnersRequired, leftForAdding = leftForAdding });
            }
            else
            {
                //If all winners prizes are added to DB
                var currentContest = this.Data.Contests.All().FirstOrDefault(c => c.Id == contestId);
                currentContest.Flag = Flag.Active;
                this.Data.SaveChanges();
                return this.RedirectToAction("Details", "Contest", new { id = contestId });
            }
        }


        private void LoadCategories()
        {
            this.ViewBag.Categories = this.Data.Categories
                                       .All()
                                       .Select(x => new SelectListItem
                                       {
                                           Value = x.Id.ToString(),
                                           Text = x.Name
                                       });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadImage(ImageBindingModel model, int contestId)
        {

            if (ModelState.IsValid)
            {
                if (model.Upload != null && model.Upload.ContentLength > 0)
                {
                    CloudBlockBlob photo = await this.UploadBlobAsync(model.Upload);
                    var userId = this.User.Identity.GetUserId();
                    var user = this.Data.Users.All().FirstOrDefault(u => u.Id == userId);

                    //var paths = Helpers.UploadImages.UploadImage(user.UserName, model.Upload, false);

                    var image = new Image
                    {
                        Title = model.Title,
                        Description = model.Description,
                        PostedOn = DateTime.Now,
                        Author = user,
                        PictureUrl = photo.Uri.ToString(),
                        ContestId = contestId
                        //PictureUrl = Dropbox.Download(paths[0], "Contest"),
                        //ThumbnailUrl = Dropbox.Download(paths[1], "Thumbnails")
                    };

                    this.Data.Images.Add(image);
                    this.Data.SaveChanges();
                    this.TempData["Success"] = new[] { "Image successfully added." };
                }

                return this.RedirectToAction("Details", "Contest", new { id = contestId });

            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid model");
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Participate(int contestId)
        {
            var contest = this.Data.Contests
                                .All()
                                .FirstOrDefault(c => c.Id == contestId);

            var loggedUserId = this.User.Identity.GetUserId();
            var loggedUser = this.Data.Users
                            .All()
                            .FirstOrDefault(u => u.Id == loggedUserId);

            if (contest != null)
            {
                contest.Participants.Add(loggedUser);
                this.Data.SaveChanges();

                return this.RedirectToAction("Details", "Contest", new { id = contestId });
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        private async Task<CloudBlockBlob> UploadBlobAsync(HttpPostedFileBase imageFile)
        {

            string blobName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var imageBlob = imagesContainer.GetBlockBlobReference(blobName);

            using (var fileStream = imageFile.InputStream)
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
            }

            return imageBlob;
        }
    }
}