namespace PhotoContest.Web.Controllers
{
    using System.Web.Mvc;
    using PhotoContest.Data;

    public abstract class BaseController : Controller
    {
        protected BaseController(IPhotoContestData data)
        {
            this.Data = data;
        }

        protected IPhotoContestData Data { get; private set; }
    }
}