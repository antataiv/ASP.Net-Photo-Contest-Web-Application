namespace PhotoContest.Web.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("notifications")]
    public class NotificationsHub : Hub
    {
        public void SendNotification(string notification)
        {
            this.Clients.Others.receiveNotification(notification);
        }
    }
}