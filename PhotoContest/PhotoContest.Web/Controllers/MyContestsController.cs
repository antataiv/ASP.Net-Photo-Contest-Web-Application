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
    using System.Net;
    using PhotoContest.Models;
    using System.Collections.Generic;
    using Microsoft.AspNet.SignalR;
    using PhotoContest.Web.Hubs;
    using System.ComponentModel.DataAnnotations;


    [System.Web.Mvc.Authorize]
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
                .OrderBy(c => c.Flag.ToString())
                .ThenBy(c => c.ParticipationStrategy.ToString())
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

            var participatingContests =
                cont.AsQueryable()
                .ProjectTo<ParticipatingContestsViewModel>()
                .Where(c => c.Flag.Equals("Active"))
                .ToPagedList(page ?? 1, 3);


            return View(participatingContests);
        }

        [Display(Name = "Invite")]
        public ActionResult AddParticipants(int? id)
        {
            if (id == null)
            {
                return this.RedirectToAction("Error404", "Home");
            }

            var contest = this.Data.Contests.Find(id);
            if (contest == null)
            {
                return this.RedirectToAction("Error404", "Home");
            }

            this.ViewBag.ContestId = contest.Id;

            return this.View();
        }

        public ActionResult AddUserToContest(int? contestId, string userId)
        {
            if (contestId == null)
            {
                return this.RedirectToAction("Error404", "Home");
            }

            var contest = this.Data.Contests.Find(contestId);
            if (contest == null)
            {
                return this.RedirectToAction("Error404", "Home");
            }

            var user = this.Data.Users.Find(userId);

            var isParticipate = contest.Participants.FirstOrDefault(p => p.UserName == user.UserName);
            if (isParticipate == null)
            {
                contest.Participants.Add(user);
                this.Data.SaveChanges();
                return this.RedirectToAction("Details", "Contest", new { id = contest.Id });
            }

            return this.RedirectToAction("Details", "Contest", new { id = contest.Id });
            //return this.PartialView("_AddUserToContest", );

        }

        [HttpGet]
        public ActionResult Search()
        {
            return PartialView("_SearchFormPartial");
        }

        [HttpPost]
        public ActionResult Search(string query)
        {

            var allUsers = this.Data.Users.All().ToList();
            var result = allUsers
                .Where(u => u.UserName.ToLower().StartsWith(query))
                .OrderBy(u => u.UserName)
                .ToList();


            List<UserViewModel> userViewModels = new List<UserViewModel>();

            foreach (var u in result)
            {
                UserViewModel model = new UserViewModel() { Id = u.Id, Username = u.UserName };
                userViewModels.Add(model);
            }

            //return Json(userViewModels, JsonRequestBehavior.AllowGet);
            return View("_SearchResultsPartial", userViewModels);

        }

        //[ValidateAntiForgeryToken]

        public ActionResult Edit(EditContestBindingModel model, int id)
        {
            var contestToEdit = this.Data.Contests.Find(id);

            if (ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (contestToEdit == null)
            {
                return this.RedirectToAction("Error404", "Home");
            }

            if (model.Name != null)
            {
                contestToEdit.Name = model.Name;
            }

            contestToEdit.DeadlineStrategy = model.DeadlineStrategy;
            if (model.EndDate != null)
            {
                contestToEdit.EndDate = model.EndDate;
            }

            if (model.ParticipantsLimit != null)
            {
                contestToEdit.ParticipantsLimit = model.ParticipantsLimit;
            }

            this.Data.SaveChanges();

            return this.View();
        }


        public ActionResult DismissContest(int id)
        {


            var contest = this.Data.Contests.All()
                .FirstOrDefault(c => c.Id == id);

            if (contest == null)
            {
                return this.RedirectToAction("Error404", "Home");
            }

            contest.Flag = Flag.Inactive;

            this.Data.SaveChanges();

            var html = HttpContext.Server.HtmlEncode(contest.Name);

            SendFinalizedMessage(string.Format("Contest {0} has been dismissed. No winners will be selected for this contest.", html));

            return this.RedirectToAction("Index", "MyContests");
        }

        //new version
        public ActionResult Finalize(int id)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                return this.RedirectToAction("Error404", "Home");
            }

            var winnersCount = contest.NumberOfPrizes.GetValueOrDefault();

            var winnersNames = this.Data.Images
                .All()
                .Where(i => i.ContestId == id && i.isDeleated == false)
                .OrderByDescending(i => i.Ratings.Sum(r => r.Value))
                .Take(winnersCount)
                .Select(i => i.Author)
                .ToList();

            var winnerPrizes = this.Data.Prizes
                .All()
                .Where(p => p.ContestId == id)
                .OrderBy(p => p.Position)
                .ToList();

            int counter = 0;
            foreach (var item in winnerPrizes)
            {
                if (counter >= winnersNames.Count())
                {
                    break;
                }
                Prize currentPrize = this.Data.Prizes.All().FirstOrDefault(p => p.Id == item.Id);
                currentPrize.Winner = winnersNames[counter];
                counter++;
            }

            contest.Flag = Flag.Past;
            this.Data.SaveChanges();

            var PrizesWithWinners = this.Data.Prizes
                .All()
                .Where(p => p.ContestId == id)
                .OrderBy(p => p.Position)
                .Project()
                .To<PrizeViewModel>()
                .ToList();

            var html = HttpContext.Server.HtmlEncode(contest.Name);

            SendFinalizedMessage(string.Format("Contest {0} has been finalized.", html));

            return this.View(PrizesWithWinners);
        }
        private void SendFinalizedMessage(string message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            hubContext.Clients.All.receiveNotification(message);
            //return this.Content("Notification sent.");
        }
    }
}