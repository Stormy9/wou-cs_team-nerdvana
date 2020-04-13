using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            petopiaUser.UserID = loggedID;
            petopiaUser.FirstName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.FirstName).First();
            petopiaUser.LastName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.LastName).First();
            petopiaUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsOwner).First();
            petopiaUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsProvider).First();
            petopiaUser.ResState = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResState).First();

            //This stuff probably won't be displayed in the profile but will be put into the model just in case
            petopiaUser.MainPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.MainPhone).First();
            petopiaUser.AltPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.AltPhone).First();
            petopiaUser.ResAddress01 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResAddress01).First();
            petopiaUser.ResAddress02 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResAddress02).First();
            petopiaUser.ResCity = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResCity).First();
            petopiaUser.ResZipcode = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResZipcode).First();


            petopiaUser.ProfilePhoto = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ProfilePhoto).First();

            //We might not have these so we want to see if we get a result back before populating...
            if (db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
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

            //Tests to make getting pets easier
            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.PetOwnerID).First();
                petopiaUser.PetList = db.Pets.Where(x => x.PetOwnerID == ownerID).Select(n => new PetInfo
                    {
                        PetName = n.PetName,
                        Species = n.Species,
                        Gender = n.Gender,
                        PetID = n.PetID
                    }).ToList();
            }

            //Check if badges exist and put in ViewBag.
            DAL.UserBadge UserBadges = db.UserBadges.Where(x => x.UserID == loggedID).FirstOrDefault();
            if (UserBadges == null)
            {
                ViewBag.Badges = false;
            } 
            else
            {
                ViewBag.Badges = true;
                ViewBag.DogOwner = UserBadges.DogOwner;
                ViewBag.DogProvider = UserBadges.DogProvider;
                ViewBag.CatOwner = UserBadges.CatOwner;
                ViewBag.CatProvider = UserBadges.CatProvider;
                ViewBag.BirdOwner = UserBadges.BirdOwner;
                ViewBag.BirdProvider = UserBadges.BirdProvider;
            }


            return View(petopiaUser);
        }

        // GET: ProfilePage/Edit
        public ActionResult Edit()
        {
            var identityID = User.Identity.GetUserId();
            var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();

            ProfileViewModel petopiaUser = new ProfileViewModel();

            petopiaUser.FirstName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.FirstName).First();
            petopiaUser.LastName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.LastName).First();
            petopiaUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsOwner).First();
            petopiaUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsProvider).First();
            petopiaUser.ResState = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResState).First();

            petopiaUser.MainPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.MainPhone).First();
            petopiaUser.AltPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.AltPhone).First();
            petopiaUser.ResAddress01 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResAddress01).First();
            petopiaUser.ResAddress02 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResAddress02).First();
            petopiaUser.ResCity = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResCity).First();
            petopiaUser.ResZipcode = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ResZipcode).First();

            //We might not have these so we want to see if we get a result back before populating...
            if (db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
            { //If we get 1, then we know this user is a CareProvider
                petopiaUser.ExperienceDetails = db.CareProviders.Where(x => x.UserID == loggedID).Select(x => x.ExperienceDetails).First();
            }
            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                petopiaUser.NeedsDetails = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.NeedsDetails).First();
                petopiaUser.AccessInstructions = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.AccessInstructions).First();
            }

            return View(petopiaUser);
        }

        // POST: ProfilePage/Edit
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityID = User.Identity.GetUserId();
                var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();
                //Need to split up model into 3 objects to fit into db
                //PetopiaUser
                DAL.PetopiaUser currentUser = new DAL.PetopiaUser();
                currentUser.UserID = loggedID;
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.ASPNetIdentityID = identityID;
                currentUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsOwner).First();
                currentUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsProvider).First();
                currentUser.MainPhone = model.MainPhone;
                currentUser.AltPhone = model.AltPhone;
                currentUser.ResAddress01 = model.ResAddress01;
                currentUser.ResAddress02 = model.ResAddress02;
                currentUser.ResCity = model.ResCity;
                currentUser.ResState = model.ResState;
                currentUser.ResZipcode = model.ResZipcode;

                //Need to put the bools back into the model just in case we have to return
                model.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsOwner).First();
                model.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.IsProvider).First();
                //save PetopiaUser into db
                if (model.UserProfilePicture != null)
                {
                    if (model.UserProfilePicture.ContentLength > (4 * 1024 * 1024))
                    {
                        ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                        return View(model);
                    }
                    if (!(model.UserProfilePicture.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("CustomError", "Image must be in jpeg format.");
                        return View(model);
                    }
                    byte[] data = new byte[model.UserProfilePicture.ContentLength];
                    model.UserProfilePicture.InputStream.Read(data, 0, model.UserProfilePicture.ContentLength);
                    currentUser.ProfilePhoto = data;
                }
                else //If no pic was uploaded, we need to seed the current profile pic into our user
                {
                    currentUser.ProfilePhoto = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.ProfilePhoto).First();
                }
                

                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();

                //Care Provider
                if (db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
                { //If we get 1, then we know this user is a CareProvider
                    DAL.CareProvider currentProvider = new DAL.CareProvider();
                    currentProvider.CareProviderID = db.CareProviders.Where(x => x.UserID == loggedID).Select(x => x.CareProviderID).First();
                    currentProvider.AverageRating = db.CareProviders.Where(x => x.UserID == loggedID).Select(x => x.AverageRating).First();
                    currentProvider.ExperienceDetails = model.ExperienceDetails;
                    currentProvider.UserID = loggedID;
                    //Save CareProvider into db
                    db.Entry(currentProvider).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //Pet Owner
                if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
                { //If we get 1, then we know this user is a PetOwner
                    DAL.PetOwner currentOwner = new DAL.PetOwner();
                    currentOwner.PetOwnerID = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.PetOwnerID).First();
                    currentOwner.AverageRating = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.AverageRating).First();
                    currentOwner.NeedsDetails = model.NeedsDetails;
                    currentOwner.AccessInstructions = model.AccessInstructions;
                    currentOwner.UserID = loggedID;
                    //Save PetOwner into db
                    db.Entry(currentOwner).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            //If we got here something bad happened, return model
            return View(model);
        }
    }
}