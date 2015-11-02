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
            if (model!=null && this.ModelState.IsValid)
            {
                model.UserId = this.User.Identity.GetUserId();
                model.PostedOn=DateTime.Now;
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
    }
}