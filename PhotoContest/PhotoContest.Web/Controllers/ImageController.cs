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
    }
}