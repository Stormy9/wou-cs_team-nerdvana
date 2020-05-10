using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Petopia.DAL;
using Petopia.Models;

namespace Petopia.Controllers
{
    public class PetOwnersController : Controller
    {
        private PetOwnerContext db = new PetOwnerContext();
        private PetopiaContext pdb = new PetopiaContext();

        // GET: PetOwners
        public ActionResult AdminIndex()
        {
            return View(db.PetOwners.ToList());
        }

        //===============================================================================
        // GET: PetOwners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Models.PetOwner petOwner = db.PetOwners.Find(id);
            if (petOwner == null)
            {
                return HttpNotFound();
            }

            return View(petOwner);
        }

        //===============================================================================
        // GET: PetOwners/Create
        public ActionResult Create()
        {
            return View();
        }
        //-------------------------------------------------------------------------------
        // POST: PetOwners/Create
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PetOwnerID,AverageRating,GeneralNeeds,HomeAccess,UserID")] DAL.PetOwner petOwner)
        {
            if (ModelState.IsValid)
            {
                //Changing Current user to a Pet Owner
                var identityID = User.Identity.GetUserId();

                DAL.PetopiaUser currentUser = pdb.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).First();

                currentUser.IsOwner = true;
                pdb.Entry(currentUser).State = EntityState.Modified;
                

                //Roles.AddUserToRole(currentUser.ASPNetIdentityID, "Owner");
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                var theUser = UserManagerExtensions.FindByName(userManager, currentUser.ASPNetIdentityID);

                UserManagerExtensions.AddToRole(userManager, identityID, "Owner");
                //needs details, access instructions
                
                pdb.SaveChanges();

                petOwner.UserID = pdb.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                                  .Select(x => x.UserID).First();

                pdb.PetOwners.Add(petOwner);

                pdb.SaveChanges();

                return RedirectToAction("Index", "ProfilePage");
            }

            return View(petOwner);
        }

        //===============================================================================
        // GET: PetOwners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Models.PetOwner petOwner = db.PetOwners.Find(id);
            if (petOwner == null)
            {
                return HttpNotFound();
            }

            return View(petOwner);
        }
        //-------------------------------------------------------------------------------
        // POST: PetOwners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PetOwnerID,AverageRating,GeneralNeeds,HomeAccess,UserID")] DAL.PetOwner petOwner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(petOwner).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(petOwner);
        }

        //===============================================================================
        // GET: PetOwners/Delete/5
        [Authorize(Roles = "Owner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Models.PetOwner petOwner = db.PetOwners.Find(id);

            if (petOwner == null)
            {
                return HttpNotFound();
            }

            return View(petOwner);
        }
        //-------------------------------------------------------------------------------
        // POST: PetOwners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.PetOwner petOwner = db.PetOwners.Find(id);
            db.PetOwners.Remove(petOwner);
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
    }
}
