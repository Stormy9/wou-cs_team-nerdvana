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

namespace Petopia.Controllers
{
    public class PetopiaUsersController : Controller
    {
        private PetopiaUserContext db = new PetopiaUserContext();

        // GET: PetopiaUsers
        public ActionResult Index()
        {
            return View(db.PetopiaUsers.ToList());
        }

        // GET: PetopiaUsers/Details/5
        public ActionResult Details(int? id)
        {
            var identityID = User.Identity.GetUserId();
            var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).Select(x => x.UserID).First();
            
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

        // GET: PetopiaUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PetopiaUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Password,FirstName,LastName,ASPNetIdentityID,IsOwner,IsProvider,MainPhone,AltPhone,ResAddress01,ResAddress02,ResCity,ResState,ResZipcode,ProfilePhoto")] PetopiaUser petopiaUser)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                petopiaUser.ASPNetIdentityID = id;
                db.PetopiaUsers.Add(petopiaUser);
                db.SaveChanges();
                return RedirectToAction("ChooseRole", "Account");  
            }

            return View(petopiaUser);
        }

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

        // POST: PetopiaUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PetopiaUser model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

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
