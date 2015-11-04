using Microsoft.AspNet.SignalR;
using PhotoContest.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoContest.Web.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Notifications
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendNotification(string notification)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            hubContext.Clients.All.receiveNotification(notification);
            return this.Content("Notification sent.");
        }
    }
}