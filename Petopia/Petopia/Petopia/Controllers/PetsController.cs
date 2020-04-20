using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.DAL;
using Petopia.Models.ViewModels;

namespace Petopia.Controllers
{
    public class PetsController : Controller
    {
        private PetopiaContext db = new PetopiaContext();

        //===============================================================================
        // GET: Pets
        public ActionResult Index()
        {
            var pets = db.Pets.Include(p => p.PetOwner);

            return View(pets.ToList());
        }

        //===============================================================================
        // GET: Pets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pet pet = db.Pets.Find(id);

            if (pet == null)
            {
                return HttpNotFound();
            }

            return View(pet);
        }

        //===============================================================================
        // GET: Pets/Create
        public ActionResult Create()
        {
            // pick list for rating?  like for 1 thru 5?
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating");

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View();
        }

        //-------------------------------------------------------------------------------
        // POST: Pets/Create
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details:  https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetPicViewModel model)
        {
            Pet pet = new Pet();
            if (ModelState.IsValid)
            {
                
                var identityID = User.Identity.GetUserId();
                var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();
                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.PetOwnerID).First();

                pet.PetName = model.PetName;
                pet.Species = model.Species;
                pet.Breed = model.Breed;
                pet.Gender = model.Gender;
                pet.Altered = model.Altered;
                pet.Birthdate = model.Birthdate;
                pet.Weight = model.Weight;
                pet.HealthConcerns = model.HealthConcerns;
                pet.BehaviorConcerns = model.BehaviorConcerns;
                pet.PetAccess = model.PetAccess;
                pet.EmergencyContactName = model.EmergencyContactName;
                pet.EmergencyContactPhone = model.EmergencyContactPhone;
                pet.NeedsDetails = model.NeedsDetails;
                pet.PetCaption = model.PetCaption;
                pet.PetBio = model.PetBio;

                pet.PetOwnerID = ownerID;

                //For profile picture
                if (model.PetPhoto != null)
                {
                    if (model.PetPhoto.ContentLength > (4 * 1024 * 1024))
                    {
                        ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                        return View(model);
                    }

                    if (!(model.PetPhoto.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("CustomError", "Image must be in jpeg format.");
                        return View(model);
                    }

                    byte[] data = new byte[model.PetPhoto.ContentLength];

                    model.PetPhoto.InputStream.Read(data, 0, model.PetPhoto.ContentLength);

                    pet.PetPhoto = data;
                }

                db.Pets.Add(pet);
                db.SaveChanges();

                return RedirectToAction("Index", "ProfilePage");
            }

            // pick list for rating -- like 1 thru 5
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View(model);
        }

        //===============================================================================
        // GET: Pets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pet pet = db.Pets.Find(id);

            if (pet == null)
            {
                return HttpNotFound();
            }

            PetPicViewModel model = new PetPicViewModel();

            model.PetName = pet.PetName;
            model.Species = pet.Species;
            model.Breed = pet.Breed;
            model.Gender = pet.Gender;
            model.Altered = pet.Altered;
            model.Birthdate = pet.Birthdate;
            model.Weight = pet.Weight;
            model.HealthConcerns = pet.HealthConcerns;
            model.BehaviorConcerns = pet.BehaviorConcerns;
            model.PetAccess = pet.PetAccess;
            model.EmergencyContactName = pet.EmergencyContactName;
            model.EmergencyContactPhone = pet.EmergencyContactPhone;
            model.NeedsDetails = pet.NeedsDetails;
            model.PetCaption = pet.PetCaption;
            model.PetBio = pet.PetBio;
            model.PetID = pet.PetID;


            // pick-list for rating -- 1 thru 5
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View(model);
        }
        //-------------------------------------------------------------------------------
        // POST: Pets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details:  https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PetPicViewModel model)
        {
            Pet pet = new Pet();
            if (ModelState.IsValid)
            {
                var identityID = User.Identity.GetUserId();
                var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();
                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.PetOwnerID).First();

                

                pet.PetOwnerID = ownerID;
                pet.PetName = model.PetName;
                pet.Species = model.Species;
                pet.Breed = model.Breed;
                pet.Gender = model.Gender;
                pet.Altered = model.Altered;
                pet.Birthdate = model.Birthdate;
                pet.Weight = model.Weight;
                pet.HealthConcerns = model.HealthConcerns;
                pet.BehaviorConcerns = model.BehaviorConcerns;
                pet.PetAccess = model.PetAccess;
                pet.EmergencyContactName = model.EmergencyContactName;
                pet.EmergencyContactPhone = model.EmergencyContactPhone;
                pet.NeedsDetails = model.NeedsDetails;
                pet.PetCaption = model.PetCaption;
                pet.PetBio = model.PetBio;

                if (model.PetPhoto != null)
                {
                    if (model.PetPhoto.ContentLength > (4 * 1024 * 1024))
                    {
                        ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                        return View(model);
                    }

                    if (!(model.PetPhoto.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("CustomError", "Image must be in jpeg format.");
                        return View(model);
                    }

                    byte[] data = new byte[model.PetPhoto.ContentLength];

                    model.PetPhoto.InputStream.Read(data, 0, model.PetPhoto.ContentLength);

                    pet.PetPhoto = data;
                }
                else //If no pic was uploaded, we need to seed the current profile pic into our user
                {
                    pet.PetPhoto = db.Pets.Where(x => x.PetID == pet.PetID).Select(x => x.PetPhoto).FirstOrDefault();
                }


                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "ProfilePage");
            }

            // pick list for rating -- like 1 thru 5
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View(model);
        }

        //===============================================================================
        // GET: Pets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pet pet = db.Pets.Find(id);

            if (pet == null)
            {
                return HttpNotFound();
            }

            return View(pet);
        }
        //-------------------------------------------------------------------------------
        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = db.Pets.Find(id);
            db.Pets.Remove(pet);
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
        //------------ BOY/GIRL DROP-DOWN LIST IN 'CREATE (and edit) ATHLETE' -----------
        //
        private IList<SelectListItem> genderSelectList = new List<SelectListItem>
        {
            new SelectListItem
                { Value = "girl", Text = "girl" },
            new SelectListItem
                { Value = "boy", Text = "boy" },
            new SelectListItem
                { Value = "boy (altered)", Text = "boy (altered)" },
            new SelectListItem
                { Value = "girl (altered)", Text = "girl (altered)" }
        };
        //===============================================================================
    }
}
