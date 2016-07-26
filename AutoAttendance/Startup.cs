using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoAttendance.Startup))]
namespace AutoAttendance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
