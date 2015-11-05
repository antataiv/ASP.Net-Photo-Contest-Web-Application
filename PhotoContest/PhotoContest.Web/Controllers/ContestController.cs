using PhotoContest.Models.Enums;

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
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.AspNet.SignalR;
    using PhotoContest.Web.Hubs;

    [ValidateInput(false)]
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
                .OrderByDescending(x => x.StartDate)
                .ProjectTo<PastContestViewModel>()
                .Where(c => c.Flag.Equals("Past"))
                .ToPagedList(page ?? 1, 3);

            return this.View(inactiveContests);

        }

        public ActionResult InactiveContests(int? page)
        {
            var currLoggedUserId = this.User.Identity.GetUserId();

            var inactiveContests = this.Data.Contests
                .All()
                .OrderByDescending(x => x.StartDate)
                .Where(c => c.CreatorId.Equals(currLoggedUserId))
                .ProjectTo<InactiveContestViewModel>()
                .Where(c => c.Flag.Equals("Inactive"))
                .ToPagedList(page ?? 1, 3);

            return this.View(inactiveContests);

        }

        public ActionResult ContestsByUser(string userName, int? page)
        {
            User userId = this.Data.Users.All().Where(u => u.UserName == userName).FirstOrDefault();

            var usersContests = this.Data.Contests
                .All()

                .Where(uc => uc.CreatorId == userId.Id)
                .OrderByDescending(uc => uc.StartDate)
                .Project()
                .To<ContestViewModelIndex>().ToPagedList(page ?? 1, 3);

            this.ViewBag.userName = userName;
            return this.View(usersContests);
        }

        [System.Web.Mvc.Authorize]
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
            if (contest != null)
            {
                var contestViewModel = Mapper.Map<ContestDetailsViewModel>(contest);

                contestViewModel.CreatorName = contest.Creator.UserName;

                contestViewModel.Participants = this.Data.Images
                    .All()
                    .Where(img => img.ContestId == id && img.isDeleated == false)
                    .Select(u => u.Author.Id)
                    .Distinct()
                    .Count();

                var numberOfPrizesInDB = this.Data.Prizes.All().Where(i => i.ContestId == contestViewModel.Id).Count();
                this.ViewBag.NumOfPrizesInDB = numberOfPrizesInDB;
                this.ViewBag.NumOfWinners = contestViewModel.NumberOfPrizes;

                return this.View(contestViewModel);
            }
            else
            {
                return this.RedirectToAction("Error404", "Home");
            }
        }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContestBindingModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                var loggedUserId = this.User.Identity.GetUserId();
                var newContest = Mapper.Map<Contest>(model);
                newContest.StartDate = DateTime.Now;
                newContest.CreatorId = loggedUserId;

                if (model.RewardStrategy == RewardStrategy.One)
                {
                    newContest.NumberOfPrizes = 1;
                }

                var creator = this.Data.Users
                                    .All()
                                    .FirstOrDefault(u => u.Id == loggedUserId);

                this.Data.Contests.Add(newContest);
                newContest.Participants.Add(creator);
                this.Data.SaveChanges();

                return this.RedirectToAction("Details", "Contest", new { newContest.Id });
            }

            this.LoadCategories();

            return this.View(model);
        }

        //new version
        [System.Web.Mvc.Authorize]
        public ActionResult CreatePrizes(int contestId, int numOfWinnersRequired, int leftForAdding)
        {
            this.ViewBag.leftForAdding = leftForAdding;
            this.ViewBag.ContestId = contestId;
            this.ViewBag.numOfWinnersRequired = numOfWinnersRequired;
            return this.View();
        }

        [HttpPost]
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
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

                SendContestCreatedNotification(string.Format("New contest \"{0}\" has been created.", currentContest.Name));

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
        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadImage(ImageBindingModel model, int contestId)
        {

            if (ModelState.IsValid)
            {
                var contest = this.Data.Contests.All().FirstOrDefault(c => c.Id == contestId);


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

                    if (!contest.Participants.Contains(user))
                    {
                        contest.Participants.Add(user);
                    }


                    this.Data.Images.Add(image);
                    this.Data.SaveChanges();
                    this.TempData["Success"] = new[] { "Image successfully added." };
                }

                return this.RedirectToAction("Details", "Contest", new { id = contestId });

            }

            return this.RedirectToAction("Error404", "Home");
        }



        [HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult Participate(string userId, int contestId)
        {
            var contest = this.Data.Contests.All()
                                .FirstOrDefault(c => c.Id == contestId);

            //var isItLast = contest.ParticipantsLimit - 1 == contest.Participants.Count;

            var loggedUserId = this.User.Identity.GetUserId();
            var userToAdd = this.Data.Users
                            .All()
                            .FirstOrDefault(u => u.Id == loggedUserId);

            if (userId != null)
            {
                userToAdd = this.Data.Users.Find(userId);
            }

            if (contest != null)
            {
                //if (contest.Participants.Contains(userToAdd))
                //{
                //    this.ViewBag.AlreadyParticipatesMessage = "This user already participates";

                //    return this.Content("This user already participates");
                //}
                contest.Participants.Add(userToAdd);

                if (IsContestFinished(contest))
                {
                    contest.Flag = Flag.Past;
                }

                this.Data.SaveChanges();

                return this.RedirectToAction("Details", "Contest", new { id = contestId });
            }

            return this.RedirectToAction("Error404", "Home");
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

        private bool IsContestFinished(Contest contest)
        {
            if (contest.DeadlineStrategy == DeadlineStrategy.ByTime)
            {
                var isOver = DateTime.Now.Date >= contest.EndDate;
                if (isOver)
                {
                    return true;
                }
            }
            else
            {
                var isFull = contest.ParticipantsLimit == contest.Participants.Count;
                if (isFull)
                {
                    return true;
                }
            }

            return false;
        }

        private void SendContestCreatedNotification(string message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            hubContext.Clients.All.receiveNotification(message);

        }
    }
}