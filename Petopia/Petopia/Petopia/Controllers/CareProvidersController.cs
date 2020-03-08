using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.Models;
using Petopia.DAL;

namespace Petopia.Controllers
{
    public class CareProvidersController : Controller
    {
        private CareProviderContext db = new CareProviderContext();
        private PetopiaContext pdb = new PetopiaContext();

        // GET: CareProviders
        public ActionResult Index()
        {
            return View(db.CareProviders.ToList());
        }

        // GET: CareProviders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CareProvider careProvider = db.CareProviders.Find(id);
            if (careProvider == null)
            {
                return HttpNotFound();
            }
            return View(careProvider);
        }

        // GET: CareProviders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CareProviders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CareProviderID,AverageRating,ExperienceDetails,UserID")] DAL.CareProvider careProvider)
        {
            if (ModelState.IsValid)
            {

                //Changing Current user to a Pet Owner
                var identityID = User.Identity.GetUserId();
                DAL.PetopiaUser currentUser = pdb.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).First();
                currentUser.IsProvider = true;
                pdb.Entry(currentUser).State = EntityState.Modified;
                pdb.SaveChanges();

                careProvider.UserID = pdb.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();
                pdb.CareProviders.Add(careProvider);
                pdb.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(careProvider);
        }

        // GET: CareProviders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CareProvider careProvider = db.CareProviders.Find(id);
            if (careProvider == null)
            {
                return HttpNotFound();
            }
            return View(careProvider);
        }

        // POST: CareProviders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CareProviderID,AverageRating,ExperienceDetails,UserID")] DAL.CareProvider careProvider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(careProvider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(careProvider);
        }

        // GET: CareProviders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CareProvider careProvider = db.CareProviders.Find(id);
            if (careProvider == null)
            {
                return HttpNotFound();
            }
            return View(careProvider);
        }

        // POST: CareProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.CareProvider careProvider = db.CareProviders.Find(id);
            db.CareProviders.Remove(careProvider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
