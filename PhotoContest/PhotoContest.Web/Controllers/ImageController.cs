using PhotoContest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoContest.Common.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PagedList;
using PagedList.Mvc;
using PhotoContest.Web.ViewModels;
using PhotoContest.Web.Models.BindingModels;
using PhotoContest.Models;

namespace PhotoContest.Web.Controllers
{
    public class ImageController : BaseController
    {
        public ImageController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult MyImages(int? page)
        {
            var userId = this.User.Identity.GetUserId();

            var myImages = this.Data.Images.All().Where(i => i.Author.Id.Equals(userId)).OrderBy(i => i.PostedOn)
                .Project()
                .To<MyImagesViewModel>()
                .ToPagedList(page ?? 1, 6);

            return View(myImages);
        }

        public ActionResult Details(int Id)
        {
            var image = this.Data.Images.All().Where(i => i.Id == Id).Project().To<ImageDetailsViewModel>().FirstOrDefault();

            return this.View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(CommentBindingModel model)
        {
            if (model != null && this.ModelState.IsValid)
            {
                model.UserId = this.User.Identity.GetUserId();
                model.PostedOn = DateTime.Now;
                var comment = Mapper.Map<Comment>(model);
                var author = this.Data.Users.All().FirstOrDefault(u => u.Id == model.UserId);
                comment.Author = author;

                this.Data.Comments.Add(comment);
                this.Data.SaveChanges();

                var commentDb = this.Data.Comments
                    .All()
                    .Where(x => x.Id == comment.Id)
                    .Project()
                    .To<CommentViewModel>()
                    .FirstOrDefault();

                return this.PartialView("DisplayTemplates/CommentViewModel", commentDb);
            }


            return this.Json("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Vote(int imageId)
        {
            var image = this.Data.Images
                            .All()
                            .FirstOrDefault(i => i.Id == imageId);

            if (image != null)
            {
                var currUserId = this.User.Identity.GetUserId();
                var currUser = this.Data.Users
                                .All()
                                .FirstOrDefault(u => u.Id == currUserId);

                var userHasVoted = image.Ratings.Any(r => r.Author.Id == currUser.Id);
                if (!userHasVoted)
                {
                    this.Data.Ratings.Add(new Rating
                    {
                        ImageId = image.Id,
                        Author = currUser,
                        Value = 1
                    });

                    this.Data.SaveChanges();
                }

                var votesCount = image.Ratings.Sum(v => v.Value);

                return this.RedirectToAction("Details", "Contest", new { id = image.ContestId });
            }

            return new EmptyResult();
        }
    }
}