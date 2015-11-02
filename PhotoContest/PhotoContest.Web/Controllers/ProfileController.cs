using PhotoContest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AutoMapper.QueryableExtensions;
using PhotoContest.Web.ViewModels;
using AutoMapper;
using PhotoContest.Common.Mappings;

namespace PhotoContest.Web.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public ProfileController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var userId= this.User.Identity.GetUserId();

            var profileInfo = this.Data.Users
                .All()
                .Where(u => u.Id.Equals(userId))
                .Project()
                .To<UserProfileViewModel>().FirstOrDefault();

            this.ViewBag.NumOfMyContests = this.Data.Contests.All().Where(c => c.CreatorId.Equals(userId)).Count();

            return View(profileInfo);
        }
    }
}