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

        //===============================================================================
        // GET: UserBadge/Create
        public ActionResult Create()
        {
            string id = User.Identity.GetUserId();
            
            ViewBag.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id)
                                             .Select(x => x.IsOwner).First();

            ViewBag.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id)
                                                .Select(x => x.IsProvider).First();

            return View();
        }
        //-------------------------------------------------------------------------------
        // POST: UserBadge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DAL.UserBadge UserBadge)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();

                int currentUserID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id)
                                                   .Select(x => x.UserID).First();

                UserBadge.UserID = currentUserID;

                db.UserBadges.Add(UserBadge);
                db.SaveChanges();

                return RedirectToAction("Index", "ProfilePage");
            }

            return View(UserBadge);
        }
        //===============================================================================
        // GET: CareProviders/Edit/5
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();

            int currentUserID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id)
                                               .Select(x => x.UserID).First();

            int badgeUserID = db.UserBadges.Where(x => x.UserID == currentUserID)
                                           .Select(x => x.UserBadgeID).First();

            ViewBag.IsOwner = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id)
                                             .Select(x => x.IsOwner).First();

            ViewBag.IsProvider = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id)
                                                .Select(x => x.IsProvider).First();

            DAL.UserBadge UserBadges = db.UserBadges.Find(badgeUserID);

            if (UserBadges == null)
            {
                return HttpNotFound();
            }

            return View(UserBadges);
        }
        //-------------------------------------------------------------------------------
        // POST: CareProviders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DAL.UserBadge userBadges)
        {
            string id = User.Identity.GetUserId();

            userBadges.UserID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == id)
                                               .Select(x => x.UserID).First();

            userBadges.UserBadgeID = db.UserBadges.Where(x => x.UserID == userBadges.UserID)
                                                  .Select(x => x.UserBadgeID).First();

            if (ModelState.IsValid)
            {
                db.Entry(userBadges).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index", "ProfilePage");
            }

            return View(userBadges);
        }
        //===============================================================================
    }
}