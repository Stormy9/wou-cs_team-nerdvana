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
using SimpleZipCode;
using static Petopia.Models.ViewModels.ProfileViewModel;

namespace Petopia.Controllers
{
    public class ProfilePageController : Controller
    {
        private PetopiaContext db = new PetopiaContext();

        //===============================================================================
        //                                      DISPLAY PROFILE PAGE -- PRIVATE\USER VIEW
        //===============================================================================
        // GET: ProfilePage                                          
        public ActionResult Index()
        {
            // this is the user's ASPNetIdentityID:
            var identityID = User.Identity.GetUserId();

            // this is FINDS that user within our Petopia table & pulls their PetopiaUserID
            var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                          .Select(x => x.UserID).FirstOrDefault();


            ProfileViewModel petopiaUser = new ProfileViewModel();

            //populating the viewmodel with a join
            /*petopiaUser = db.PetopiaUsers.Join(db.PetOwners,
                                                pu => pu.UserID,
                                                po => po.UserID,
                                                (pu, po) => new {PetUse = pu, PetOwn = po }) */
            //linq isnt populating correctly right now so we're doing it manually (TEMP FIX)

            petopiaUser.UserID = loggedID;

            petopiaUser.FirstName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                   .Select(x => x.FirstName).First();
            petopiaUser.LastName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.LastName).First();

            // user role
            petopiaUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.IsOwner).First();
            petopiaUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                    .Select(x => x.IsProvider).First();


            // This stuff probably won't be displayed in the profile 
            //    but will be put into the model just in case
            petopiaUser.MainPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                   .Select(x => x.MainPhone).First();
            petopiaUser.AltPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.AltPhone).First();
            petopiaUser.ResAddress01 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                      .Select(x => x.ResAddress01).First();
            petopiaUser.ResAddress02 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                      .Select(x => x.ResAddress02).First();
            petopiaUser.ResCity = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.ResCity).First();
            petopiaUser.ResState = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.ResState).First();
            petopiaUser.ResZipcode = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                    .Select(x => x.ResZipcode).First();


            // user profile picture!
            petopiaUser.ProfilePhoto = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                      .Select(x => x.ProfilePhoto).First();

            petopiaUser.UserCaption = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                     .Select(x => x.UserCaption).First();


            // user profile fun stuff
            petopiaUser.Tagline = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.Tagline).First();

            petopiaUser.GeneralLocation = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                         .Select(x => x.GeneralLocation).First();

            petopiaUser.UserBio = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.UserBio).First();

            //---------------------------------------------------------------------------
            //                             Owner Needs & Access -AND- Provider Experience
            //---------------------------------------------------------------------------
            //We might not have these so we want to see if we get a result back before populating...
            if (db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
            {
                //If we get 1, then we know this user is a CareProvider
                petopiaUser.ExperienceDetails = db.CareProviders.Where(x => x.UserID == loggedID)
                                                                .Select(x => x.ExperienceDetails).First();

                petopiaUser.CareProviderID = db.CareProviders.Where(x => x.UserID == loggedID)
                                                                .Select(x => x.CareProviderID).First();
            }
            //---------------------------------------------------------
            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                petopiaUser.GeneralNeeds = db.PetOwners.Where(x => x.UserID == loggedID)
                                                       .Select(x => x.GeneralNeeds).First();

                petopiaUser.HomeAccess = db.PetOwners.Where(x => x.UserID == loggedID)
                                                     .Select(x => x.HomeAccess).First();

                petopiaUser.PetOwnerID = db.PetOwners.Where(x => x.UserID == loggedID)
                                                     .Select(x => x.PetOwnerID).First();
            }
            //---------------------------------------------------------------------------
            //                                             OWNER\PROVIDER AVERAGE RATINGS
            //---------------------------------------------------------------------------
            // trying to get average rating for Pet Owners & Care Providers
            if (db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
            {
                //If we get 1, then we know this user is a CareProvider
                // 'loggedID' == that user's PetopiaUserID
                //
                // get PetopiaUser's CareProviderID:
                var thisCP_ID = db.CareProviders.Where(x => x.UserID == loggedID)
                                                .Select(x => x.CareProviderID).FirstOrDefault();

                // try for this Care Provider's Ratings to Average:
                var thisCP_Avg_Rating = db.CareTransactions.Where(x => x.CareProviderID == thisCP_ID)
                                                           .Average(x => x.PC_Rating);

                //petopiaUser.ProviderAverageRating = thisCP_Avg_Rating;
            }
            //---------------------------------------------------------
            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                // get PetopiaUser's PetOwnerID:
                var thisPO_ID = db.PetOwners.Where(x => x.UserID == loggedID)
                                            .Select(x => x.PetOwnerID).FirstOrDefault();

                // try for this Pet Owner's Ratings to Average:
                var thisPO_Avg_Rating = db.CareTransactions.Where(x => x.PetOwnerID == thisPO_ID)
                                                           .Average(x => x.PO_Rating);

                //petopiaUser.OwnerAverageRating = thisPO_Avg_Rating;
            }
            //---------------------------------------------------------------------------
            //                                                           GET PET(S) LIST!
            //---------------------------------------------------------------------------
            //Tests to make getting pets easier
            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID)
                                          .Select(x => x.PetOwnerID).First();

                petopiaUser.PetList = db.Pets.Where(x => x.PetOwnerID == ownerID)
                                             .Select(n => new PetInfo
                                             {
                                                 PetName = n.PetName,
                                                 Species = n.Species,
                                                 Breed = n.Breed,
                                                 Gender = n.Gender,
                                                 Birthdate = n.Birthdate,
                                                 PetID = n.PetID,
                                                 PetPhoto = n.PetPhoto
                                             }).ToList();
            }
            //---------------------------------------------------------------------------
            //                                                  GET OWNER\PROVIDER BADGES
            //---------------------------------------------------------------------------
            //Check if badges exist and put in ViewBag.
            DAL.UserBadge UserBadges = db.UserBadges.Where(x => x.UserID == loggedID)
                                                    .FirstOrDefault();
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
                ViewBag.FishOwner = UserBadges.FishOwner;
                ViewBag.FishProvider = UserBadges.FishProvider;
                ViewBag.HorseOwner = UserBadges.HorseOwner;
                ViewBag.HorseProvider = UserBadges.HorseProvider;
                ViewBag.LivestockOwner = UserBadges.LivestockOwner;
                ViewBag.LivestockProvider = UserBadges.LivestockProvider;
                ViewBag.RabbitOwner = UserBadges.RabbitOwner;
                ViewBag.RabbitProvider = UserBadges.RabbitProvider;
                ViewBag.ReptileOwner = UserBadges.ReptileOwner;
                ViewBag.ReptileProvider = UserBadges.ReptileProvider;
                ViewBag.RodentOwner = UserBadges.RodentOwner;
                ViewBag.RodentProvider = UserBadges.RodentProvider;
                ViewBag.OtherOwner = UserBadges.OtherOwner;
                ViewBag.OtherProvider = UserBadges.OtherProvider;
            }
            
            //---------------------------------------------------------------------------
            //                                              ZIP LOGIC for SCROLLY-WINDOWS
            //---------------------------------------------------------------------------
            //Logic for populating scroll area on profiles based around zip code proximity.
            var ZipCodes = ZipCodeSource.FromMemory().GetRepository();

            var OwnerLocation = ZipCodes.Get(petopiaUser.ResZipcode);
            var ZipCodesNearOwner = ZipCodes.RadiusSearch(OwnerLocation, 10);

            List<String> NearbyZipsList = new List<String>();

            foreach (ZipCode zip in ZipCodesNearOwner)
            {
                NearbyZipsList.Add(zip.PostalCode);
            }

            List<String> SharedZips = new List<String>();

            foreach (var i in SharedZips)
            {

            }

            ViewBag.ZipList = String.Join(",", NearbyZipsList.ToArray());

            //---------------------------------------------------------------------------
            //                           SCROLLY-WINDOWS -- on the right of profile pages
            //---------------------------------------------------------------------------
            //
            //IF IM BOTH THEN SHOW LIST OF BOTH OWNERS AND PROVIDERS I'VE WORKED WITH IN PAST
            if (petopiaUser.IsOwner == true && petopiaUser.IsProvider == true)
            {
                petopiaUser.PetopiaUsersList = (from ct in db.CareTransactions
                    where ct.CareProviderID == petopiaUser.CareProviderID

                    join co in db.CareProviders on ct.PetOwnerID equals co.CareProviderID
                    join pu in db.PetopiaUsers on co.UserID equals pu.UserID
                    join ub in db.UserBadges on pu.UserID equals ub.UserID

                    select new ProfileViewModel.petopiaUsersInfo
                    {
                        UserID = pu.UserID,
                        FirstName = pu.FirstName,
                        LastName = pu.LastName,
                        GeneralLocation = pu.GeneralLocation,
                        ProfilePic = pu.ProfilePhoto,
                        UserBadgeID = ub.UserBadgeID,
                        DogOwner = ub.DogOwner,
                        CatOwner = ub.CatOwner,
                        BirdOwner = ub.BirdOwner,
                        FishOwner = ub.FishOwner,
                        HorseOwner = ub.HorseOwner,
                        LivestockOwner = ub.LivestockOwner,
                        RabbitOwner = ub.RabbitOwner,
                        ReptileOwner = ub.ReptileOwner,
                        RodentOwner = ub.RodentOwner,
                        OtherOwner = ub.OtherOwner
                    }).Distinct().ToList();
            }
            //IF IM ONLY A PET OWNER, SHOW LIST OF PROVIDERS
            else if (petopiaUser.IsOwner == true)
            {
                petopiaUser.PetopiaUsersList = (from ct in db.CareTransactions
                    where ct.CareProviderID == petopiaUser.CareProviderID

                    join co in db.CareProviders on ct.PetOwnerID equals co.CareProviderID
                    join pu in db.PetopiaUsers on co.UserID equals pu.UserID
                    join ub in db.UserBadges on pu.UserID equals ub.UserID

                    select new ProfileViewModel.petopiaUsersInfo
                    {
                        UserID = pu.UserID,
                        FirstName = pu.FirstName,
                        LastName = pu.LastName,
                        GeneralLocation = pu.GeneralLocation,
                        ProfilePic = pu.ProfilePhoto,
                        UserBadgeID = ub.UserBadgeID,
                        DogOwner = ub.DogOwner,
                        CatOwner = ub.CatOwner,
                        BirdOwner = ub.BirdOwner,
                        FishOwner = ub.FishOwner,
                        HorseOwner = ub.HorseOwner,
                        LivestockOwner = ub.LivestockOwner,
                        RabbitOwner = ub.RabbitOwner,
                        ReptileOwner = ub.ReptileOwner,
                        RodentOwner = ub.RodentOwner,
                        OtherOwner = ub.OtherOwner
                    }).Distinct().ToList();
            }
            //IF IM ONLY CARE PROVIDER, SHOW LIST OF PET OWNERS
            else if (petopiaUser.IsProvider == true)
            {
                petopiaUser.PetopiaUsersList = (from ct in db.CareTransactions
                        where ct.CareProviderID == petopiaUser.CareProviderID

                        join co in db.PetOwners on ct.PetOwnerID equals co.PetOwnerID
                        join pu in db.PetopiaUsers on co.UserID equals pu.UserID
                        join ub in db.UserBadges on pu.UserID equals ub.UserID

                        select new ProfileViewModel.petopiaUsersInfo
                        {
                            UserID = pu.UserID,
                            FirstName = pu.FirstName,
                            LastName = pu.LastName,
                            GeneralLocation = pu.GeneralLocation,
                            ProfilePic = pu.ProfilePhoto,
                            UserBadgeID = ub.UserBadgeID,
                            DogOwner = ub.DogOwner,
                            CatOwner = ub.CatOwner,
                            BirdOwner = ub.BirdOwner,
                            FishOwner = ub.FishOwner,
                            HorseOwner = ub.HorseOwner,
                            LivestockOwner = ub.LivestockOwner,
                            RabbitOwner = ub.RabbitOwner,
                            ReptileOwner = ub.ReptileOwner,
                            RodentOwner = ub.RodentOwner,
                            OtherOwner = ub.OtherOwner,
                            DogProvider = ub.DogProvider,
                            CatProvider = ub.CatProvider,
                            BirdProvider = ub.BirdProvider,
                            FishProvider = ub.FishProvider,
                            HorseProvider = ub.HorseProvider,
                            LivestockProvider = ub.LivestockProvider,
                            RabbitProvider = ub.RabbitProvider,
                            ReptileProvider = ub.ReptileProvider,
                            RodentProvider = ub.RodentProvider,
                            OtherProvider = ub.OtherProvider
                        }).ToList();
            }
            //---------------------------------------------------------------------------
            //                                CHECK FOR PENDING & INCOMPLETE APPOINTMENTS
            //---------------------------------------------------------------------------
            // CHECK FOR PENDING APPOINTMENTS -- CARE PROVIDERS
            // IFF => we get 1, then we know this logged-in user is a CareProvider
            if (db.CareProviders.Where(cp => cp.UserID == loggedID).Count() == 1)
            {
                // then get this user's CareProviderID.....
                int cp_ID = db.CareProviders.Where(cp => cp.UserID == loggedID)
                                            .Select(cpID => cpID.CareProviderID).FirstOrDefault();
                // so THIS works.....
                ViewBag.cp_ID = cp_ID;
                    
                // then try to pull a test ct_ID:
                var test_ct_ID = db.CareTransactions.Where(ct => ct.CareProviderID == cp_ID)
                                                    .Select(ct => ct.TransactionID).FirstOrDefault();
                // and THIS works.....
                ViewBag.test_ct_ID = test_ct_ID;

                // does this care provider have any pending appointments?? 
                if (db.CareTransactions.Where(ct => (ct.CareProviderID == cp_ID) && (ct.Pending)).Count() > 0)
                {
                    // and this works!
                    bool anyPending = true;
                    ViewBag.anyPending = anyPending;
                }

                // CHECK FOR INCOMPLETE APPOINTMENTS -- 
                // does this care provider have un-completed appts (ratings\comments)?
                if (db.CareTransactions.Where(ct => (ct.CareProviderID == cp_ID) 
                                              && (!ct.Completed_PO)
                                              && (ct.EndDate < DateTime.Today)).Count() > 0)
                {
                    bool incompleteAppts_CP = true;
                    ViewBag.incompleteAppts_CP = incompleteAppts_CP;
                }
            }
            //---------------------------------------------------------
            // PET OWNER SIDE -- CHECK FOR INCOMPLETE APPOINTMENTS.....
            if (db.PetOwners.Where(po => po.UserID == loggedID).Count() == 1)
            {
                // get this user's PetOwnerID.....
                int po_ID = db.PetOwners.Where(po => po.UserID == loggedID)
                                        .Select(poID => poID.PetOwnerID).FirstOrDefault();
                // so THIS works.....
                ViewBag.po_ID = po_ID;

                // then try to pull a test ct_ID:
                var test_ct_ID = db.CareTransactions.Where(ct => ct.CareProviderID == po_ID)
                                                    .Select(ct => ct.TransactionID).FirstOrDefault();
                // and THIS works.....
                ViewBag.test_ct_ID = test_ct_ID;

                // does this pet owner have un-completed appointments (ratings\comments)?
                if (db.CareTransactions.Where(ct => (ct.PetOwnerID == po_ID) 
                                              && (ct.EndDate < DateTime.Today) 
                                              && (!ct.Completed_CP)).Count() > 0)
                {
                    bool incompleteAppts_PO = true;
                    ViewBag.incompleteAppts_PO = incompleteAppts_PO;
                }
            }
            //---------------------------------------------------------
            return View(petopiaUser);
        }
        //===============================================================================
        //                                            EDIT PROFILE & ACCOUNT STUFF -- GET
        //===============================================================================
        // GET: ProfilePage/EditMyStuff
        public ActionResult EditMyStuff()
        {
            var identityID = User.Identity.GetUserId();
            var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                          .Select(x => x.UserID).First();


            ProfileViewModel petopiaUser = new ProfileViewModel();

            petopiaUser.FirstName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                   .Select(x => x.FirstName).First();
            petopiaUser.LastName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.LastName).First();


            // user role
            petopiaUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.IsOwner).First();
            petopiaUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                    .Select(x => x.IsProvider).First();


            // user account\private info
            petopiaUser.MainPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                   .Select(x => x.MainPhone).First();
            petopiaUser.AltPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.AltPhone).First();
            petopiaUser.ResAddress01 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                      .Select(x => x.ResAddress01).First();
            petopiaUser.ResAddress02 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                      .Select(x => x.ResAddress02).First();
            petopiaUser.ResCity = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.ResCity).First();
            petopiaUser.ResState = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.ResState).First();
            petopiaUser.ResZipcode = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                    .Select(x => x.ResZipcode).First();


            // user profile fun stuff
            petopiaUser.UserCaption = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                     .Select(x => x.UserCaption).First();

            petopiaUser.Tagline = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.Tagline).First();

            petopiaUser.GeneralLocation = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                         .Select(x => x.GeneralLocation).First();

            petopiaUser.UserBio = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.UserBio).First();


            //---------------------------------------------------------------------------
            //                             Owner Needs & Access -AND- Provider Experience
            //---------------------------------------------------------------------------
            //We might not have these so we want to see if we get a result back 
            //     before populating...
            if (db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
            {
                //If we get 1, then we know this user is a CareProvider
                petopiaUser.ExperienceDetails = db.CareProviders.Where(x => x.UserID == loggedID)
                                                                .Select(x => x.ExperienceDetails).First();
            }

            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                petopiaUser.GeneralNeeds = db.PetOwners.Where(x => x.UserID == loggedID)
                                                       .Select(x => x.GeneralNeeds).First();

                petopiaUser.HomeAccess = db.PetOwners.Where(x => x.UserID == loggedID)
                                                     .Select(x => x.HomeAccess).First();
            }
            //---------------------------------------------------------------------------

            return View(petopiaUser);
        }
        //-------------------------------------------------------------------------------
        //                                           EDIT PROFILE & ACCOUNT STUFF -- POST
        //-------------------------------------------------------------------------------
        // POST: ProfilePage/EditProfile
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyStuff(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var identityID = User.Identity.GetUserId();
                var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                              .Select(x => x.UserID).First();


                //Need to split up model into 3 objects to fit into db
                //-----------------------------------------------------------------------
                //                             PetopiaUser - common to Owners & Providers
                //-----------------------------------------------------------------------
                DAL.PetopiaUser currentUser = new DAL.PetopiaUser();

                currentUser.UserID = loggedID;

                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;


                // ID & user roles
                currentUser.ASPNetIdentityID = identityID;
                currentUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                     .Select(x => x.IsOwner).First();
                currentUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                        .Select(x => x.IsProvider).First();


                // user account\private info
                currentUser.MainPhone = model.MainPhone;
                currentUser.AltPhone = model.AltPhone;
                currentUser.ResAddress01 = model.ResAddress01;
                currentUser.ResAddress02 = model.ResAddress02;
                currentUser.ResCity = model.ResCity;
                currentUser.ResState = model.ResState;
                currentUser.ResZipcode = model.ResZipcode;


                // user profile fun stuff
                currentUser.UserCaption = model.UserCaption;
                currentUser.GeneralLocation = model.GeneralLocation;
                currentUser.UserBio = model.UserBio;
                currentUser.Tagline = model.Tagline;

                //-----------------------------------------------------------------------
                //Need to put bools back into the model just in case we have to return
                //                                                             User Roles
                //-----------------------------------------------------------------------
                model.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                               .Select(x => x.IsOwner).First();

                model.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.IsProvider).First();

                //-----------------------------------------------------------------------
                //                                                     Profile Pic stuff!
                //-----------------------------------------------------------------------
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

                //If no pic was uploaded, we need to seed the current profile pic into our user
                else
                {
                    currentUser.ProfilePhoto = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                              .Select(x => x.ProfilePhoto).First();
                }
                //-----------------------------------------------------

                // save ***PetopiaUser*** into db
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();

                //-----------------------------------------------------------------------
                //                                          Care Provider - specific info
                //-----------------------------------------------------------------------
                if (db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
                {
                    //If we get 1, then we know this user is a CareProvider
                    DAL.CareProvider currentProvider = new DAL.CareProvider();

                    currentProvider.CareProviderID = db.CareProviders.Where(x => x.UserID == loggedID)
                                                                     .Select(x => x.CareProviderID).First();


                    currentProvider.AverageRating = db.CareProviders.Where(x => x.UserID == loggedID)
                                                                    .Select(x => x.AverageRating).First();

                    currentProvider.ExperienceDetails = model.ExperienceDetails;

                    currentProvider.UserID = loggedID;

                    //Save CareProvider into db
                    db.Entry(currentProvider).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //-----------------------------------------------------------------------
                //                                              Pet Owner - specific info
                //-----------------------------------------------------------------------
                if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
                {
                    //If we get 1, then we know this user is a PetOwner
                    DAL.PetOwner currentOwner = new DAL.PetOwner();

                    currentOwner.PetOwnerID = db.PetOwners.Where(x => x.UserID == loggedID)
                                                          .Select(x => x.PetOwnerID).First();


                    currentOwner.AverageRating = db.PetOwners.Where(x => x.UserID == loggedID)
                                                             .Select(x => x.AverageRating).First();

                    currentOwner.GeneralNeeds = model.GeneralNeeds;

                    currentOwner.HomeAccess = model.HomeAccess;

                    currentOwner.UserID = loggedID;

                    //Save PetOwner into db
                    db.Entry(currentOwner).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //-----------------------------------------------------------------------
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            //If we got here something bad happened, return model
            return View(model);

        }  // END 'EditProfile()' POST
        //===============================================================================
        //                                                             VISIT PROFILE PAGE
        //===============================================================================
        // GET: ProfilePage                                            
        public ActionResult VisitProfile(int loggedID)
        {
            // originally -- from the Pet Profile Page -- which was the only place we had
            //     a link to `VisitProfile(id)` to start with..... anyway we were passing 
            //     in the owner's `PetOwnerID` instead of their `PetopiaUserID`.....
            //       that was getting us the incorrect ProfilePage!
            // SO...
            //  make sure, wherever we link to `VisitProfile(id)`, that we are passing
            //  in the `PetopiaUserID` and not `PetOwnerID` or `CareProviderID`
            //     -- that can be done via ViewBag like i did in the Pet Profile Page


            var identityID = db.PetopiaUsers.Where(x => x.UserID == loggedID)
                                            .Select(x => x.ASPNetIdentityID)
                                            .First();


            ProfileViewModel petopiaUser = new ProfileViewModel();

            //populating the viewmodel with a join
            /*petopiaUser = db.PetopiaUsers.Join(db.PetOwners,
                                                pu => pu.UserID,
                                                po => po.UserID,
                                                (pu, po) => new {PetUse = pu, PetOwn = po }) */
            //linq isnt populating correctly right now so we're doing it manually (TEMP FIX)

            petopiaUser.UserID = loggedID;

            petopiaUser.FirstName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                   .Select(x => x.FirstName).First();
            petopiaUser.LastName = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.LastName).First();

            // user role
            petopiaUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.IsOwner).First();
            petopiaUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                    .Select(x => x.IsProvider).First();


            // This stuff probably won't be displayed in the profile 
            //                      but will be put into the model just in case
            petopiaUser.MainPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                   .Select(x => x.MainPhone).First();
            petopiaUser.AltPhone = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.AltPhone).First();
            petopiaUser.ResAddress01 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                      .Select(x => x.ResAddress01).First();
            petopiaUser.ResAddress02 = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                      .Select(x => x.ResAddress02).First();
            petopiaUser.ResCity = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.ResCity).First();
            petopiaUser.ResState = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.ResState).First();
            petopiaUser.ResZipcode = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                    .Select(x => x.ResZipcode).First();


            // user profile picture!
            petopiaUser.ProfilePhoto = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                      .Select(x => x.ProfilePhoto).First();

            petopiaUser.UserCaption = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                     .Select(x => x.UserCaption).First();


            // user profile fun stuff
            petopiaUser.Tagline = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.Tagline).First();

            petopiaUser.GeneralLocation = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                         .Select(x => x.GeneralLocation).First();

            petopiaUser.UserBio = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.UserBio).First();

            //---------------------------------------------------------------------------
            //                             Owner Needs & Access -AND- Provider Experience
            //---------------------------------------------------------------------------
            //We might not have these so we want to see if we get a result back before populating...
            if (db.CareProviders.Where(x => x.UserID == loggedID).Count() == 1)
            {
                //If we get 1, then we know this user is a CareProvider
                petopiaUser.ProviderAverageRating = db.CareProviders.Where(x => x.UserID == loggedID)
                                                                    .Select(x => x.AverageRating).First();

                petopiaUser.ExperienceDetails = db.CareProviders.Where(x => x.UserID == loggedID)
                                                                .Select(x => x.ExperienceDetails).First();
            }
            //---------------------------------------------------------
            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                petopiaUser.OwnerAverageRating = db.PetOwners.Where(x => x.UserID == loggedID)
                                                             .Select(x => x.AverageRating).First();

                petopiaUser.GeneralNeeds = db.PetOwners.Where(x => x.UserID == loggedID)
                                                       .Select(x => x.GeneralNeeds).First();

                petopiaUser.HomeAccess = db.PetOwners.Where(x => x.UserID == loggedID)
                                                     .Select(x => x.HomeAccess).First();
            }
            //---------------------------------------------------------------------------
            //                                                           GET PET(S) LIST!
            //---------------------------------------------------------------------------
            //Tests to make getting pets easier
            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID)
                                          .Select(x => x.PetOwnerID).First();

                petopiaUser.PetList = db.Pets.Where(x => x.PetOwnerID == ownerID)
                                             .Select(n => new PetInfo
                                             {
                                                 PetName = n.PetName,
                                                 Species = n.Species,
                                                 Breed = n.Breed,
                                                 Gender = n.Gender,
                                                 Birthdate = n.Birthdate,
                                                 PetID = n.PetID,
                                                 PetPhoto = n.PetPhoto,
                                                 PetOwnersID = n.PetOwnerID
                                             }).ToList();
            }
            //---------------------------------------------------------------------------
            //                                                      OWNER\PROVIDER BADGES
            //---------------------------------------------------------------------------
            //Check if badges exist and put in ViewBag.
            DAL.UserBadge UserBadges = db.UserBadges.Where(x => x.UserID == loggedID)
                                                    .FirstOrDefault();

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
                ViewBag.FishOwner = UserBadges.FishOwner;
                ViewBag.FishProvider = UserBadges.FishProvider;
                ViewBag.HorseOwner = UserBadges.HorseOwner;
                ViewBag.HorseProvider = UserBadges.HorseProvider;
                ViewBag.LivestockOwner = UserBadges.LivestockOwner;
                ViewBag.LivestockProvider = UserBadges.LivestockProvider;
                ViewBag.RabbitOwner = UserBadges.RabbitOwner;
                ViewBag.RabbitProvider = UserBadges.RabbitProvider;
                ViewBag.ReptileOwner = UserBadges.ReptileOwner;
                ViewBag.ReptileProvider = UserBadges.ReptileProvider;
                ViewBag.RodentOwner = UserBadges.RodentOwner;
                ViewBag.RodentProvider = UserBadges.RodentProvider;
                ViewBag.OtherOwner = UserBadges.OtherOwner;
                ViewBag.OtherProvider = UserBadges.OtherProvider;
            }
            return View(petopiaUser);
        }
        //---------------------------------------------------------------------------
        //                                             OWNER\PROVIDER AVERAGE RATINGS
        //---------------------------------------------------------------------------
        // trying to get average rating for Pet Owners & Care Providers


        //---------------------------------------------------------------------------
        //                                              ZIP LOGIC for SCROLLY-WINDOWS
        //---------------------------------------------------------------------------
        //Logic for populating scroll area on profiles based around zip code proximity.


        //---------------------------------------------------------------------------
        //                           SCROLLY-WINDOWS -- on the right of profile pages
        //---------------------------------------------------------------------------
        //
        //IF IM BOTH THEN SHOW LIST OF BOTH OWNERS AND PROVIDERS I'VE WORKED WITH IN PAST



        //IF IM ONLY A PET OWNER, SHOW LIST OF PROVIDERS



        //IF IM ONLY CARE PROVIDER, SHOW LIST OF PET OWNERS

        //
        //
        //===============================================================================
        //===============================================================================
    }
}