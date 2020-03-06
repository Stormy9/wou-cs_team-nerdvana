using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.DAL;
using Petopia.Models;
using Petopia.Models.ViewModels;
using static Petopia.Models.ViewModels.ProfileViewModel;

namespace Petopia.Controllers
{
    public class ProfilePageController : Controller
    {
        private PetopiaContext db = new PetopiaContext();
        // GET: ProfilePage
        public ActionResult Index()
        {
            var identityID = User.Identity.GetUserId();
            var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();

            ProfileViewModel petopiaUser = new ProfileViewModel();
            //populating the viewmodel with a join
            /*
            petopiaUser = db.PetopiaUsers.Join(db.PetOwners,
                                                pu => pu.UserID,
                                                po => po.UserID,
                                                (pu, po) => new {PetUse = pu, PetOwn = po }) */
            //linq isnt populating correctly right now so we're doing it manually (TEMP FIX)
            petopiaUser.FirstName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.FirstName).First();
            petopiaUser.LastName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.LastName).First();
            petopiaUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsOwner).First();
            petopiaUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsProvider).First();
            petopiaUser.ResState = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResState).First();
            //We might not have these so we want to see if we get a result back before populating...
            if(db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
            { //If we get 1, then we know this user is a CareProvider
            petopiaUser.ProviderAverageRating = db.CareProviders.Where(x => x.UserID == loggedID).Select(x => x.AverageRating).First();
            petopiaUser.ExperienceDetails = db.CareProviders.Where(x => x.UserID == loggedID).Select(x => x.ExperienceDetails).First();
            }
            if(db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
            petopiaUser.OwnerAverageRating = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.AverageRating).First();
            petopiaUser.NeedsDetails = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.NeedsDetails).First();
            petopiaUser.AccessInstructions = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.AccessInstructions).First();
            }

            return View(petopiaUser);
        }
    }
}