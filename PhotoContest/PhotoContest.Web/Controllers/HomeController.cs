using DropNet;

namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using AutoMapper.QueryableExtensions;
    using Data;
    using PhotoContest.Web.ViewModels;
    using AutoMapper;
    using PhotoContest.Common.Mappings;
    using PagedList;
    using PagedList.Mvc;
    using System.Linq;
    using Microsoft.AspNet.Identity;

    [RequireHttps]
    public class HomeController : BaseController
    {
        public HomeController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult Index(int? page)
        {
            var activeContests = this.Data.Contests
                .All()
                .OrderBy(x => x.StartDate)
                .ProjectTo<ContestViewModelIndex>()
                .Where(c => c.Flag.Equals("Active"))
                .ToPagedList(page ?? 1, 3);

            string currentUserId = this.User.Identity.GetUserId();

            this.ViewBag.NumberOfCreatedContests = this.Data.Contests.All().Where(c => c.CreatorId.Equals(currentUserId)).Count();



            return this.View(activeContests);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}