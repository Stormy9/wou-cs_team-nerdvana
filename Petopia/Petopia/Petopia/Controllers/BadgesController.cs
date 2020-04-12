using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.DAL;
using Petopia.Models;

namespace Petopia.Controllers
{
    public class BadgesController : Controller
    {
        private PetopiaContext db = new PetopiaContext();

        // GET: UserBadge/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: UserBadge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DAL.UserBadge UserBadge)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                int currentUserID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id).Select(x => x.UserID).First();
                UserBadge.UserID = currentUserID;
                db.UserBadges.Add(UserBadge);
                db.SaveChanges();

                return RedirectToAction("ChooseRole", "Account");
            }

            return View(UserBadge);
        }

        // GET: CareProviders/Edit/5
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();
            int currentUserID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id).Select(x => x.UserID).First();
            DAL.UserBadge UserBadges = db.UserBadges.Find(currentUserID);
            if (UserBadges == null)
            {
                return HttpNotFound();
            }
            return View(UserBadges);
        }

        // POST: CareProviders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DAL.UserBadge userBadges)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userBadges).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userBadges);
        }
    }
}