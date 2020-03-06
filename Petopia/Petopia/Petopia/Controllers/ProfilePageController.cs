﻿using System;
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
            /*
             * Need to put in way to grab profile picture
             */
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
                currentUser.IsOwner = model.IsOwner;
                currentUser.IsProvider = model.IsProvider;
                currentUser.MainPhone = model.MainPhone;
                currentUser.AltPhone = model.AltPhone;
                currentUser.ResAddress01 = model.ResAddress01;
                currentUser.ResAddress02 = model.ResAddress02;
                currentUser.ResCity = model.ResCity;
                currentUser.ResState = model.ResState;
                currentUser.ResZipcode = model.ResZipcode;
                currentUser.ProfilePhoto = null; //TODO Profile Picture
                //save PetopiaUser into db
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
                return RedirectToAction("Index");
            }
            //If we got here something bad happened, return model
            return View(model);
        }
    }
}