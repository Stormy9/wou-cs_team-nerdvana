using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.DAL;
using Petopia.Models;
using Petopia.Models.ViewModels;

namespace Petopia.Controllers
{
    public class CareTransactionsController : Controller
    {
        // pull in the db through DAL\context
        private PetopiaContext db = new PetopiaContext();

        //===============================================================================
        // GET: CareTransactions
        public ActionResult Index()
        {
            // original!
            return View(db.CareTransactions.ToList());
            // like what the admins would see -- every appointment for every user
        }

        //===============================================================================
        // GET: CareTransactions/AppointmentDetails/5
        public ActionResult AppointmentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(id);
            if (careTransaction == null)
            {
                return HttpNotFound();
            }

            return View(careTransaction);
        }

        //===============================================================================
        // GET: CareTransactions/Create
        public ActionResult BookAppointment()
        {
            // *** Trying to get the logged-in user's ID, into the 'PetOwnerID'
            //     field, for when a user clicks to book a Pet Care appointment!
            //
            var identityID = User.Identity.GetUserId();

            var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                          .Select(u => u.UserID).First();

            int petOwnerID = db.PetOwners.Where(po => po.PetOwnerID == loggedID)
                                         .Select(po => po.PetOwnerID)
                                         .First();

            // then put 'petOwnerID' into the 'PetOwnerID' field of CareTransaction.....


            // *** Trying to get a list of logged-in user's pets for drop-down
            //           when owner is booking a pet care appointment
            //
            var thesePets = db.Pets.Where(po => po.PetOwnerID == loggedID);
            
            //ViewBag.UsersPets = new SelectList(db.Pets.Where(p => p.PetOwnerID = thesePets), "PetId", "PetName");


            return View();
        }
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/BookAppointment
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport," +
          "Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID," +
          "CareProviderID,PetID,NeededThisVisit")] CareTransaction careTransaction)
        {
            // the model -- or ViewModel -- acts as a type
            //CareTransactionViewModel appt = new CareTransactionViewModel();

            if (ModelState.IsValid)
            {
                //var identityID = User.Identity.GetUserId();

                //var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                //                              .Select(u => u.UserID).First();

                // gotta have a little more in here -- understand examples better!
                //appt.PetOwnerID = careTransaction.PetOwnerID;

                db.CareTransactions.Add(careTransaction);
                db.SaveChanges();

                return RedirectToAction("AppointmentConfirmation", 
                                         new { id = careTransaction.TransactionID });
            }

            return View(careTransaction);
        }

        //===============================================================================
        // GET: CareTransactions/EditAppointment/5
        public ActionResult EditAppointment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(id);
            if (careTransaction == null)
            {
                return HttpNotFound();
            }

            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/EditAppointment/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport,Charge," +
          "Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID,CareProviderID,PetID," +
            "NeededThisVisit")] CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(careTransaction).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("AppointmentConfirmation", 
                                        new { id = careTransaction.TransactionID });
            }

            return View(careTransaction);
        }

        //===============================================================================
        // GET: CareTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(id);
            if (careTransaction == null)
            {
                return HttpNotFound();
            }

            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CareTransaction careTransaction = db.CareTransactions.Find(id);
            db.CareTransactions.Remove(careTransaction);
            db.SaveChanges();

            // CHANGE THIS!!!
            return RedirectToAction("DeleteConfirmation");
        }
        //-------------------------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //===============================================================================
        // our added 'ActionResult' methods.....
        //===============================================================================
        // GET: CareTransactions/AppointmentConfirmation/5
        public ActionResult AppointmentConfirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(id);
            if (careTransaction == null)
            {
                return HttpNotFound();
            }

            return View(careTransaction);
        }

        //===============================================================================
        // GET: CareTransactions/CompleteAppointment/5
        public ActionResult CompleteAppointment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(id);
            if (careTransaction == null)
            {
                return HttpNotFound();
            }

            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/CompleteAppointment/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport," +
            " Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID," +
            "CareProviderID,PetID,NeededThisVisit")] CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(careTransaction).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("MyAppointments");
            }

            return View(careTransaction);
        }

        //===============================================================================
        // GET: CareTransactions
        public ActionResult MyAppointments()
        {
            var identityID = User.Identity.GetUserId();

            var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                          .Select(u => u.UserID).First();


            var userAppts = db.CareTransactions.Where(ct => ct.PetOwnerID == loggedID)
                                               .OrderBy(ct => ct.StartDate);
                                            // is the .Where() part right??

            return View(userAppts.ToList());


            // original!  (from scaffolding)
            //return View(db.CareTransactions.ToList());

            // OBVIOUSLY.....
            // make this so that it only returns the logged-in user's stuff!!!
        }
        //===============================================================================
        // GET: CareTransactions/MyPetsAppointments/5
        public ActionResult MyPetsAppointments(int? id)
        {
            var thisPet = db.CareTransactions.Where(ct => ct.PetID == id)
                                             .OrderBy(ct => ct.StartDate);

            return View(thisPet.ToList());


            // original!
            //return View(db.CareTransactions.ToList());

            // OBVIOUSLY.....
            // make this so that it only returns the logged-in user's PET'S stuff!!!
        }
        //===============================================================================
        // GET: CareTransactions/DeleteConfirmation
        public ActionResult DeleteConfirmation()
        {
            return View();
        }
        //===============================================================================
    }
}
