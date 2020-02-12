using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Petopia.Startup))]
namespace Petopia
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
