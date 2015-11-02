namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using PhotoContest.Data;
    using System.Linq;
    using PhotoContest.Web.ViewModels;
    using AutoMapper.QueryableExtensions;
    using PhotoContest.Web.Models.BindingModels;
    using AutoMapper;
    using PhotoContest.Common.Mappings;
    using PagedList;
    using PagedList.Mvc;

    public class MyContestsController : BaseController
    {
        public MyContestsController(IPhotoContestData data)
            : base(data)
        {
        }

        // GET: MyContests       
        public ActionResult Index()
        {
            var userId = this.User.Identity.GetUserId();

            var myContests = this.Data.Contests
                .All()
                .Where(c => userId == c.CreatorId)
                .ProjectTo<MyContestsViewModel>();

            return View(myContests);
        }

        public ActionResult ParticipatingContests(int? page)
        {
            var userId = this.User.Identity.GetUserId();
            var cont = this.Data.Images.All()
                .OrderBy(i => i.Contest.StartDate)
                .Where(i => i.Author.Id.Equals(userId))
                .Select(i => i.Contest).Distinct().ToList();

            var ParticipatingContests = cont.AsQueryable().Project().To<ParticipatingContestsViewModel>().Where(c => c.Flag.Equals("Active")).ToPagedList(page ?? 1,3);


            return View(ParticipatingContests);
        }

        public ActionResult Update(int id, EditContestBindingModel model)
        {
            return null;
        }

        public ActionResult Dismiss(int id)
        {
            return null;
        }

        public ActionResult Finalize(int id)
        {
            return null;
        }
    }
}