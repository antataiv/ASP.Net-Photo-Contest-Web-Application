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
using System.Linq;
using PagedList;
using PagedList.Mvc;
using PhotoContest.Web.ViewModels;
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

        public ActionResult Details(int? Id)
        {


            return null;
        }

        public ActionResult Vote(int imageId)
        {
            var image = this.Data.Images
                            .All()
                            .FirstOrDefault(i => i.Id == imageId);

            var currUserId = this.User.Identity.GetUserId();

            var currUser = this.Data.Users
                                .All()
                                .FirstOrDefault(u => u.Id == currUserId);

            if (image != null)
            {
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

                return this.Content(votesCount.ToString());
            }

            return new EmptyResult();
        }
    }
}