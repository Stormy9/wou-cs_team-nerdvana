using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Petopia.Models;

[assembly: OwinStartupAttribute(typeof(Petopia.Startup))]
namespace Petopia
{
    public partial class Startup
    {
        private void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Owner"))
            {
                var role = new IdentityRole();
                role.Name = "Owner";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Provider"))
            {
                var role2 = new IdentityRole();
                role2.Name = "Provider";
                roleManager.Create(role2);
            }


        }

        public void Configuration(IAppBuilder app)
        {
           //CreateRoles();
            ConfigureAuth(app);
        }
    }
}
