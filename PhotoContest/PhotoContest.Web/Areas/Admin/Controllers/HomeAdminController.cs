using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoContest.Common.Mappings;
using PhotoContest.Data;
using PhotoContest.Web.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace PhotoContest.Web.Areas.Admin.Controllers
{
    public class HomeAdminController : BaseAdminController
    {
        public HomeAdminController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var allContests = this.Data.Contests
                .All()
                .OrderBy(c => c.Flag.ToString())
                .Project()
                .To<MyContestsViewModel>();

            return this.View(allContests);
        }
    }
}