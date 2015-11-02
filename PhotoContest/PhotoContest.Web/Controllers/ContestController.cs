

namespace PhotoContest.Web.Controllers
{
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
    using PhotoContest.Web.Helpers;

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
            var contest = this.Data.Contests
                                .All()
                                .FirstOrDefault(c => c.Id == id);

            var contestViewModel = Mapper.Map<ContestDetailsViewModel>(contest);

            var userId = this.User.Identity.GetUserId();
            contestViewModel.CreatorName = contest.Creator.UserName;
            contestViewModel.UserPatricipates = this.Data.Images
                                                    .All()
                                                    .Where(img => img.ContestId == id)
                                                    .Any(i => i.Author.Id == userId);

            contestViewModel.Participants = this.Data.Images
                                                    .All()
                                                    .Where(img => img.ContestId == id)
                                                    .Select(u => u.Author.Id)
                                                    .Distinct()
                                                    .Count();

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
        public ActionResult UploadImage(ImageBindingModel model, int contestId)
        {

            if (ModelState.IsValid)
            {
                if (model.Upload != null)
                {
                    var userId = this.User.Identity.GetUserId();
                    var user = this.Data.Users.All().FirstOrDefault(u => u.Id == userId);
                    var paths = Helpers.UploadImages.UploadImage(user.UserName, model.Upload, false);

                    var image = new Image
                    {
                        Title = model.Title,
                        Description = model.Description,
                        PostedOn = DateTime.Now,
                        Author = user,
                        PictureUrl = Dropbox.Download(paths[0], "Contest"),
                        ThumbnailUrl = Dropbox.Download(paths[1], "Thumbnails"),
                        ContestId = contestId
                    };

                    this.Data.Images.Add(image);
                    this.Data.SaveChanges();
                    this.TempData["Success"] = new[] { "Image successfully added." };
                }

                return this.RedirectToAction("Details", "Contest", new { contestId });

            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid model");
        }
    }
}