using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoContest.Web.Startup))]
namespace PhotoContest.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
