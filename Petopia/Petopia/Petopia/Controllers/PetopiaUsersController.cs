using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.Models;
using Petopia.Models.ViewModels;

namespace Petopia.Controllers
{
    public class PetopiaUsersController : Controller
    {
        private PetopiaUserContext db = new PetopiaUserContext();

        //===============================================================================
        // GET: PetopiaUsers
        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            return View(db.PetopiaUsers.ToList());
        }

        //===============================================================================
        // GET: PetopiaUsers/Details/5
        public ActionResult Details(int? id)
        {
            var identityID = User.Identity.GetUserId();
            var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                          .Select(x => x.UserID).First();

            if (loggedID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PetopiaUser petopiaUser = db.PetopiaUsers.Find(loggedID);
            if (petopiaUser == null)
            {
                return HttpNotFound();
            }

            return View(petopiaUser);
        }

        //===============================================================================
        // GET: PetopiaUsers/Create
        public ActionResult Create()
        {
            return View();
        }
        //-------------------------------------------------------------------------------
        // POST: PetopiaUsers/Create
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetopiaUserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                PetopiaUser petopiaUser = new PetopiaUser();
                //------------------------------------------

                string id = User.Identity.GetUserId();
                petopiaUser.ASPNetIdentityID = id;

                petopiaUser.UserName = model.UserName;
                petopiaUser.Password = model.Password;

                petopiaUser.FirstName = model.FirstName;
                petopiaUser.LastName = model.LastName;

                petopiaUser.IsOwner = false;
                petopiaUser.IsProvider = false;

                petopiaUser.MainPhone = model.MainPhone;
                petopiaUser.AltPhone = model.AltPhone;

                petopiaUser.ResAddress01 = model.ResAddress01;
                petopiaUser.ResAddress02 = model.ResAddress02;
                petopiaUser.ResCity = model.ResCity;
                petopiaUser.ResState = model.ResState;
                petopiaUser.ResZipcode = model.ResZipcode;

                petopiaUser.UserCaption = model.UserCaption;
                petopiaUser.GeneralLocation = model.GeneralLocation;
                petopiaUser.UserBio = model.UserBio;
                petopiaUser.Tagline = model.Tagline;

                //For profile picture
                if (model.ProfilePhoto != null)
                {
                    if (model.ProfilePhoto.ContentLength > (4 * 1024 * 1024))
                    {
                        ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                        return View(model);
                    }

                    if (!(model.ProfilePhoto.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("CustomError", "Image must be in jpeg format.");
                        return View(model);
                    }

                    byte[] data = new byte[model.ProfilePhoto.ContentLength];

                    model.ProfilePhoto.InputStream.Read(data, 0, model.ProfilePhoto.ContentLength);

                    petopiaUser.ProfilePhoto = data;
                }

                //-----------------------------------------------------
                db.PetopiaUsers.Add(petopiaUser);
                db.SaveChanges();

                return RedirectToAction("ChooseRole", "Account");
            }

            return View(model);
        }
        //===============================================================================
        // GET: PetopiaUsers/Owner_Carer
        public ActionResult Owner_Carer()
        {
            return View();
        }
        //===============================================================================
        // GET: PetopiaUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PetopiaUser petopiaUser = db.PetopiaUsers.Find(id);

            if (petopiaUser == null)
            {
                return HttpNotFound();
            }

            return View(petopiaUser);
        }
        //-------------------------------------------------------------------------------
        // POST: PetopiaUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PetopiaUser model)
        {
            // just curious:  why does this ActionResult signature not have that big ol'
            // ([Bind(Include = "lots of stuff")] PetopiaUser petopiaUser)
            // thing in it -- like, say, is in the PetsController for Edit Pet?
            //
            // it's in the Create PetopiaUser, but in the PetsController it's in both

            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        //===============================================================================
        // GET: PetopiaUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PetopiaUser petopiaUser = db.PetopiaUsers.Find(id);
            if (petopiaUser == null)
            {
                return HttpNotFound();
            }

            return View(petopiaUser);
        }
        //-------------------------------------------------------------------------------
        // POST: PetopiaUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PetopiaUser petopiaUser = db.PetopiaUsers.Find(id);
            db.PetopiaUsers.Remove(petopiaUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //===============================================================================
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //===============================================================================
        //===============================================================================
        public ActionResult MyAccountDetails()
        {
            // JUST the non-public\profile *private* account info
            // modified from ProfilePageController\Index()

            var identityID = User.Identity.GetUserId();
            var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                          .Select(x => x.UserID).First();


            // This.....
            MyAccountViewModel petopiaUser = new MyAccountViewModel();


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


            petopiaUser.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                 .Select(x => x.IsOwner).First();
            petopiaUser.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                    .Select(x => x.IsProvider).First();


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

            //---------------------------------------------------------------------------

            if (db.PetOwners.Where(x => x.UserID == loggedID).Count() == 1)
            {
                // 'OwnerAverageRating' is a public-facing profile item -- this is Account Only
                // 'GeneralNeeds' is (will be) a public-facing profile item -- this is Account Only

                // 'HomeAccess' is Account Only   [=
                petopiaUser.HomeAccess = db.PetOwners.Where(x => x.UserID == loggedID)
                                                     .Select(x => x.HomeAccess).First();
            }

            return View(petopiaUser);

        } // END 'MyAccountDetails()' GET

        //===============================================================================
    }
}
