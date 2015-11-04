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
using PhotoContest.Models;

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
            var userId = this.User.Identity.GetUserId();

            var profileInfo = this.Data.Users
                .All()
                .Where(u => u.Id.Equals(userId))
                .ProjectTo<UserProfileViewModel>().FirstOrDefault();

            var myContests = this.Data.Contests
                .All()
                .Where(c => c.CreatorId==userId)
                .OrderBy(c => c.Flag.ToString())
                .ProjectTo<MyContestsViewModel>().ToList();

            var profile = new ProfileViewModel();
            profile.UserProfileViewModel = profileInfo;
            profile.MyContests = myContests;

            this.ViewBag.NumOfMyContests = this.Data.Contests.All().Where(c => c.CreatorId.Equals(userId)).Count();

            return View(profile);
        }

        public ActionResult Details(string userName)
        {
            var profileInfo = this.Data.Users
                .All()
                .Where(u => u.UserName.Equals(userName))
                .ProjectTo<UserProfileViewModel>().FirstOrDefault();

            var myContests = this.Data.Contests
                .All()
                .Where(c => c.Creator.UserName == userName && c.Flag==Flag.Active)
                .OrderByDescending(c => c.StartDate)
                .ProjectTo<MyContestsViewModel>().ToList();

            var profile = new ProfileViewModel();
            profile.UserProfileViewModel = profileInfo;
            profile.MyContests = myContests;

            this.ViewBag.NumOfMyContests = this.Data.Contests.All().Where(c => c.Creator.UserName.Equals(userName)).Count();

            return View(profile);
        }
    }
}