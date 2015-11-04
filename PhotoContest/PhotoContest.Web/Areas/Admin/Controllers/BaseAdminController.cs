
using PhotoContest.Data;
using System.Web.Mvc;
namespace PhotoContest.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public abstract class BaseAdminController : Controller
    {
        private Data.IPhotoContestData data;

        public BaseAdminController(Data.IPhotoContestData data)
        {
            // TODO: Complete member initialization
            this.Data = data;
        }


        protected IPhotoContestData Data { get; private set; }
    }
}