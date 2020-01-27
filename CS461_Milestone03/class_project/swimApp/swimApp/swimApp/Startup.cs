using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(swimApp.Startup))]
namespace swimApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
