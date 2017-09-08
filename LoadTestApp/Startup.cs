using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoadTestApp.Startup))]
namespace LoadTestApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
