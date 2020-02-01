using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using swimApp.Models;

[assembly: OwinStartupAttribute(typeof(swimApp.Startup))]
namespace swimApp
{
    public partial class Startup
    {
        private void CreateRole()
        {
            //Grab the context to edit the db
            ApplicationDbContext context = new ApplicationDbContext();
            //Make var roleManager so that we can edit roles in the identity db
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //Check if coach already exists in db, if it doesnt then create it
            if (!roleManager.RoleExists("Coach"))
            {
                var role = new IdentityRole();
                role.Name = "Coach";
                roleManager.Create(role);
            }
        }

        public void Configuration(IAppBuilder app)
        {
            CreateRole(); //Run CreateRole so that coach can be made.
            ConfigureAuth(app);
        }
    }
}
