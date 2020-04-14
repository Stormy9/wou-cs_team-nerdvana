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
            // wait..... what is this for?  at least the 'AverageRating' part?
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
        public ActionResult Create([Bind(Include = "PetID,PetName,Species,Breed,Gender,Altered,Birthdate,Weight,HealthConcerns,BehaviorConcerns,PetAccess,EmergencyContactName,EmergencyContactPhone,NeedsDetails,AccessInstructions,PetOwnerID,PetCaption,PetBio")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                var identityID = User.Identity.GetUserId();
                var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();
                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.PetOwnerID).First();

                pet.PetOwnerID = ownerID;
                db.Pets.Add(pet);
                db.SaveChanges();

                return RedirectToAction("Index", "ProfilePage");
            }

            // why is the 'AverageRating' here?
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View(pet);
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

            // still not sure what the 'AverageRating' is doing here   [=
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View(pet);
        }
        //-------------------------------------------------------------------------------
        // POST: Pets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details:  https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PetID,PetName,Species,Breed,Gender,Altered,Birthdate,Weight,HealthConcerns,BehaviorConcerns,PetAccess,EmergencyContactName,EmergencyContactPhone,NeedsDetails,AccessInstructions,PetOwnerID,PetCaption,PetBio")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                var identityID = User.Identity.GetUserId();
                var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();
                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID).Select(x => x.PetOwnerID).First();

                pet.PetOwnerID = ownerID;
                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "ProfilePage");
            }

            // the 'AverageRating' thing again?
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View(pet);
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
