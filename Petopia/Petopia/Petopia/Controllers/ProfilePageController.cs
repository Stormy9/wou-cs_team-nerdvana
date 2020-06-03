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
            //                           SCROLLY-WINDOWS -- on the right of profile pages
            //---------------------------------------------------------------------------

            if (petopiaUser.ResZipcode != null) //If the user has a zipcode then we can grab providers in their area
            {
                //Grab logged in user info
                //var identityID = User.Identity.GetUserId();
                DAL.PetopiaUser loggedUser = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).FirstOrDefault();

                //Get zipcodes near logged in user
                var ZipCodes = ZipCodeSource.FromMemory().GetRepository();

                var OwnerLocation = ZipCodes.Get(loggedUser.ResZipcode);
                var ZipCodesNearOwner = ZipCodes.RadiusSearch(OwnerLocation, 10);

                //Should never reach here, however if somehow the user has an invalid zip then this will stop that
                if (OwnerLocation == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Invalid zipcode");
                }

                //Grab the zipcodes near the user
                List<String> zipsList = new List<String>();

                foreach (ZipCode zip in ZipCodesNearOwner)
                {
                    zipsList.Add(zip.PostalCode);
                }

                ProfileViewModel tempVM = new ProfileViewModel();

                //Populate list of users within 10 miles to start scoring
                tempVM.PetopiaUsersList = (from pu in db.PetopiaUsers

                                           where zipsList.Contains(pu.ResZipcode) && pu.IsProvider

                                           join cp in db.CareProviders on pu.UserID equals cp.UserID
                                           join ub in db.UserBadges on cp.UserID equals ub.UserID

                                           select new ProfileViewModel.petopiaUsersInfo
                                           {
                                               CP_ID = cp.CareProviderID,
                                               UID = pu.UserID,
                                               Name = pu.FirstName + " " + pu.LastName,
                                               Zipcode = pu.ResZipcode,
                                               ProfilePic = pu.ProfilePhoto,

                                               ExperienceDetails = cp.ExperienceDetails,
                                               ProviderAverageRating = cp.AverageRating,
                                               GeneralLocation = pu.GeneralLocation,

                                               IsDogProvider = ub.DogProvider,
                                               IsCatProvider = ub.CatProvider,
                                               IsBirdProvider = ub.BirdProvider,
                                               IsFishProvider = ub.FishProvider,
                                               IsHorseProvider = ub.HorseProvider,
                                               IsLivestockProvider = ub.LivestockProvider,
                                               IsRabbitProvider = ub.RabbitProvider,
                                               IsReptileProvider = ub.ReptileProvider,
                                               IsRodentProvider = ub.RodentProvider,
                                               IsOtherProvider = ub.OtherProvider

                                           }).ToList();

                //Grab logged in users badges for testing

                RecViewModel.RecProviders loggedUserRec = new RecViewModel.RecProviders();

                loggedUserRec = (from pu in db.PetopiaUsers
                                 where pu.ASPNetIdentityID == identityID
                                 join ub in db.UserBadges on pu.UserID equals ub.UserID
                                 select new RecViewModel.RecProviders
                                 {
                                     IsDogProvider = ub.DogOwner,
                                     IsCatProvider = ub.CatOwner,
                                     IsBirdProvider = ub.BirdOwner,
                                     IsFishProvider = ub.FishOwner,
                                     IsHorseProvider = ub.HorseOwner,
                                     IsLivestockProvider = ub.LivestockOwner,
                                     IsRabbitProvider = ub.RabbitOwner,
                                     IsReptileProvider = ub.ReptileOwner,
                                     IsRodentProvider = ub.RodentOwner,
                                     IsOtherProvider = ub.OtherOwner
                                 }).FirstOrDefault();

                //Time to iterate through each person in the list and add 1 point for each matching badge
                //First we must initialize each persons score to 0
                foreach (var user in tempVM.PetopiaUsersList)
                {
                    user.Score = 0;

                    //Adding score for each matching badge
                    if (user.IsBirdProvider == loggedUserRec.IsBirdProvider && user.IsBirdProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsCatProvider == loggedUserRec.IsCatProvider && user.IsCatProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsDogProvider == loggedUserRec.IsDogProvider && user.IsDogProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsFishProvider == loggedUserRec.IsFishProvider && user.IsFishProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsHorseProvider == loggedUserRec.IsHorseProvider && user.IsHorseProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsLivestockProvider == loggedUserRec.IsLivestockProvider && user.IsLivestockProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsOtherProvider == loggedUserRec.IsOtherProvider && user.IsOtherProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsRabbitProvider == loggedUserRec.IsRabbitProvider && user.IsRabbitProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsReptileProvider == loggedUserRec.IsReptileProvider && user.IsReptileProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }
                    if (user.IsRodentProvider == loggedUserRec.IsRodentProvider && user.IsRodentProvider == true)
                    {
                        user.Score = user.Score + 1;
                    }

                    //multiplying score by average rating
                    //If they have gotten no ratings yet, they will be multiplied a little above average (3 stars)
                    if (user.ProviderAverageRating == null)
                    {
                        user.Score = user.Score * 3;
                    }
                    else //they have a rating
                    {
                        user.Score = user.Score * user.ProviderAverageRating;
                    }

                }
                //Add the list to our viewmodel, sorting by score
                petopiaUser.PetopiaUsersList = tempVM.PetopiaUsersList.OrderByDescending(x => x.Score).ToList();
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

            //---------------------------------------------------------------------------
            //                           SCROLLY-WINDOWS -- on the right of profile pages
            //---------------------------------------------------------------------------
            if (User.Identity.IsAuthenticated == true)
            {
                var loggedIdentityID = User.Identity.GetUserId();
                DAL.PetopiaUser loggedUser = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == loggedIdentityID).FirstOrDefault();

                //Since this is the visiting of a profile, we need to grab the logged in users info
                if (loggedUser.ResZipcode != null) //If the user has a zipcode then we can grab providers in their area
                {
                    //Get zipcodes near logged in user
                    var ZipCodes = ZipCodeSource.FromMemory().GetRepository();

                    var OwnerLocation = ZipCodes.Get(loggedUser.ResZipcode);
                    var ZipCodesNearOwner = ZipCodes.RadiusSearch(OwnerLocation, 10);

                    //Should never reach here, however if somehow the user has an invalid zip then this will stop that
                    if (OwnerLocation == null)
                    {
                        return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Invalid zipcode");
                    }

                    //Grab the zipcodes near the user
                    List<String> zipsList = new List<String>();

                    foreach (ZipCode zip in ZipCodesNearOwner)
                    {
                        zipsList.Add(zip.PostalCode);
                    }

                    ProfileViewModel tempVM = new ProfileViewModel();

                    //Populate list of users within 10 miles to start scoring
                    tempVM.PetopiaUsersList = (from pu in db.PetopiaUsers

                                               where zipsList.Contains(pu.ResZipcode) && pu.IsProvider

                                               join cp in db.CareProviders on pu.UserID equals cp.UserID
                                               join ub in db.UserBadges on cp.UserID equals ub.UserID

                                               select new ProfileViewModel.petopiaUsersInfo
                                               {
                                                   CP_ID = cp.CareProviderID,
                                                   UID = pu.UserID,
                                                   Name = pu.FirstName + " " + pu.LastName,
                                                   Zipcode = pu.ResZipcode,
                                                   ProfilePic = pu.ProfilePhoto,

                                                   ExperienceDetails = cp.ExperienceDetails,
                                                   ProviderAverageRating = cp.AverageRating,
                                                   GeneralLocation = pu.GeneralLocation,

                                                   IsDogProvider = ub.DogProvider,
                                                   IsCatProvider = ub.CatProvider,
                                                   IsBirdProvider = ub.BirdProvider,
                                                   IsFishProvider = ub.FishProvider,
                                                   IsHorseProvider = ub.HorseProvider,
                                                   IsLivestockProvider = ub.LivestockProvider,
                                                   IsRabbitProvider = ub.RabbitProvider,
                                                   IsReptileProvider = ub.ReptileProvider,
                                                   IsRodentProvider = ub.RodentProvider,
                                                   IsOtherProvider = ub.OtherProvider

                                               }).ToList();

                    //Grab logged in users badges for testing

                    RecViewModel.RecProviders loggedUserRec = new RecViewModel.RecProviders();

                    loggedUserRec = (from pu in db.PetopiaUsers
                                     where pu.ASPNetIdentityID == loggedIdentityID
                                     join ub in db.UserBadges on pu.UserID equals ub.UserID
                                     select new RecViewModel.RecProviders
                                     {
                                         IsDogProvider = ub.DogOwner,
                                         IsCatProvider = ub.CatOwner,
                                         IsBirdProvider = ub.BirdOwner,
                                         IsFishProvider = ub.FishOwner,
                                         IsHorseProvider = ub.HorseOwner,
                                         IsLivestockProvider = ub.LivestockOwner,
                                         IsRabbitProvider = ub.RabbitOwner,
                                         IsReptileProvider = ub.ReptileOwner,
                                         IsRodentProvider = ub.RodentOwner,
                                         IsOtherProvider = ub.OtherOwner
                                     }).FirstOrDefault();

                    //Time to iterate through each person in the list and add 1 point for each matching badge
                    //First we must initialize each persons score to 0
                    foreach (var user in tempVM.PetopiaUsersList)
                    {
                        user.Score = 0;

                        //Adding score for each matching badge
                        if (user.IsBirdProvider == loggedUserRec.IsBirdProvider && user.IsBirdProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsCatProvider == loggedUserRec.IsCatProvider && user.IsCatProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsDogProvider == loggedUserRec.IsDogProvider && user.IsDogProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsFishProvider == loggedUserRec.IsFishProvider && user.IsFishProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsHorseProvider == loggedUserRec.IsHorseProvider && user.IsHorseProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsLivestockProvider == loggedUserRec.IsLivestockProvider && user.IsLivestockProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsOtherProvider == loggedUserRec.IsOtherProvider && user.IsOtherProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsRabbitProvider == loggedUserRec.IsRabbitProvider && user.IsRabbitProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsReptileProvider == loggedUserRec.IsReptileProvider && user.IsReptileProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }
                        if (user.IsRodentProvider == loggedUserRec.IsRodentProvider && user.IsRodentProvider == true)
                        {
                            user.Score = user.Score + 1;
                        }

                        //multiplying score by average rating
                        //If they have gotten no ratings yet, they will be multiplied a little above average (3 stars)
                        if (user.ProviderAverageRating == null)
                        {
                            user.Score = user.Score * 3;
                        }
                        else //they have a rating
                        {
                            user.Score = user.Score * user.ProviderAverageRating;
                        }

                    }
                    //Add the list to our viewmodel, sorting by score
                    petopiaUser.PetopiaUsersList = tempVM.PetopiaUsersList.OrderByDescending(x => x.Score).ToList();
                }

            }

            return View(petopiaUser);
        }
        //---------------------------------------------------------------------------
        //                                             OWNER\PROVIDER AVERAGE RATINGS
        //---------------------------------------------------------------------------
        // trying to get average rating for Pet Owners & Care Providers








        //===============================================================================
        //===============================================================================
    }
}