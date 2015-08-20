using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(admin_mode.Startup))]
namespace admin_mode
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
