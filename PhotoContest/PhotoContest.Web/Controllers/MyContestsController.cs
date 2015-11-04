﻿
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
                .Project()
                .To<ParticipatingContestsViewModel>()
                .Where(c => c.Flag.Equals("Active"))
                .ToPagedList(page ?? 1,3);


            return View(participatingContests);
        }

        // GET: /MyContests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var contestToEdit = this.Data.Contests.Find(id);
            if (contestToEdit == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }


            return this.View(contestToEdit);
        }

        // POST: /MyContests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditContestBindingModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return null;
        }


        public ActionResult DismissContest(int id)
        {


            var contest = this.Data.Contests.All()
                .FirstOrDefault(c => c.Id == id);

            if (contest == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            contest.Flag = Flag.Inactive;

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "MyContests");
        }

        //new version
        public ActionResult Finalize(int id)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var winnersCount = contest.NumberOfPrizes.GetValueOrDefault();

            var winnersNames = this.Data.Images
                .All()
                .Where(i => i.ContestId == id && i.isDeleated == false)
                .OrderByDescending(i => i.Ratings.Sum(r => r.Value) )
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
                if (counter>= winnersNames.Count())
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
            //return this.RedirectToAction("Index", "MyContests");

            return this.View(PrizesWithWinners);
        }

        //public ActionResult Finalize(int id)
        //{
        //    var contest = this.Data.Contests.Find(id);

        //    if (contest == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        //    }

        //    var winnersCount = contest.NumberOfPrizes.GetValueOrDefault();
        //    //var winners = this.Data.Contests.All()
        //    //    .Take(winnersCount)
        //    //    .ProjectTo<PrizeViewModel>();

        //    var winners = this.Data.Images
        //        .All()
        //        .Where(i=>i.ContestId==id)
        //        .OrderByDescending(i=>i.Ratings.Sum(r=>r.Value))
        //        .Take(winnersCount)
        //        .Project()
        //        .To<WinnerImagesViewModel>().ToList();

        //    contest.Flag = Flag.Inactive;
        //    this.Data.SaveChanges();

        //    //return this.RedirectToAction("Index", "MyContests");

        //    return this.View(winners);
        //}
    }
}