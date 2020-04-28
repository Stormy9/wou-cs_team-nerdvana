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
                                          .Select(u => u.UserID).FirstOrDefault();

            int petOwnerID = db.PetOwners.Where(po => po.PetOwnerID == loggedID)
                                         .Select(po => po.PetOwnerID)
                                         .FirstOrDefault();

            // OR -- does this go in the [HttpPost] BookAppointment() ActionResult???
            // then put 'petOwnerID' into the 'PetOwnerID' field of CareTransaction.....
            // seems like it makes more sense to go here?  
            //      but it's not here in 'AddPet()'... ?

            // based off Victoria's example ..... and this works!
            //   'BookAppointment()' is now bound to the currently logged-in user!
            //
            //      why isn't this working like Corrin's AddPet() though?
            ViewBag.petOwnerID = petOwnerID;

            //
            //---------------------------------------------------------------------------
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
            if (ModelState.IsValid)
            {
                // trying like Corrin's 'AddPet()'
                // but this doesn't like that way.....  haha!
                //CareTransaction appt = new CareTransaction();

                var identityID = User.Identity.GetUserId();

                var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                              .Select(u => u.UserID).FirstOrDefault();

                int petOwnerID = db.PetOwners.Where(po => po.UserID == loggedID)
                                             .Select(po => po.PetOwnerID)
                                             .FirstOrDefault();

                // based from 'AddPet()'
                //appt.PetOwnerID = petOwnerID;

                db.CareTransactions.Add(careTransaction);   // tried w/ Add(appt) - no go
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

                return RedirectToAction("EditConfirmation", 
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

            //---------------------------------------------------------------------------
            // trying to pull the Pet's Name for display!         it worked!   =]
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName)
                                     .FirstOrDefault();

            ViewBag.PetName = thisPetName;
            //---------------------------------------------------------------------------
            // getting the Pet Carer name for display!
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName)
                                                    .FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.LastName)
                                                    .FirstOrDefault();

            ViewBag.CarerFirstName = thisCarerFirstName;
            ViewBag.CarerLastName = thisCarerLastName;
            //---------------------------------------------------------------------------
            // getting start & end dates -- to format the display
            var thisStartDate = careTransaction.StartDate;
            var thisEndDate = careTransaction.EndDate;

            var formatStartDate = thisStartDate.ToString("MM/dd/yyyy");
            var formatEndDate = thisEndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = formatStartDate;
            ViewBag.ApptEndDate = formatEndDate;

            //---------------------------------------------------------------------------

            return View(careTransaction);
        }

        //===============================================================================
        // GET: CareTransaction/EditConfirmation/5
        public ActionResult EditConfirmation(int? id)
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

            //---------------------------------------------------------------------------
            // pulling the Pet's Name for display!   
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName)
                                     .FirstOrDefault();

            ViewBag.PetName = thisPetName;
            //---------------------------------------------------------------------------
            // getting the Pet Carer name for display!
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName)
                                                    .FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.LastName)
                                                    .FirstOrDefault();

            ViewBag.CarerFirstName = thisCarerFirstName;
            ViewBag.CarerLastName = thisCarerLastName;
            //---------------------------------------------------------------------------
            // getting start & end dates -- to format the display
            var thisStartDate = careTransaction.StartDate;
            var thisEndDate = careTransaction.EndDate;

            var formatStartDate = thisStartDate.ToString("MM/dd/yyyy");
            var formatEndDate = thisEndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = formatStartDate;
            ViewBag.ApptEndDate = formatEndDate;

            //---------------------------------------------------------------------------

            return View(careTransaction);
        }
        //===============================================================================
        // GET: CareTransactions/DeleteConfirmation
        public ActionResult DeleteConfirmation()
        {
            return View();
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
            //---------------------------------------------------------------------------
            var identityID = User.Identity.GetUserId();

            var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                          .Select(u => u.UserID).FirstOrDefault();


            var userAppts = db.CareTransactions.Where(ct => ct.PetOwnerID == loggedID)
                                               .OrderBy(ct => ct.StartDate);

            //---------------------------------------------------------------------------

            
            //---------------------------------------------------------------------------

            return View(userAppts.ToList());


            // OBVIOUSLY.....
            // make this so that it only returns the logged-in user's stuff!!!
            // it seems to be doing this now, yay!
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
    }
}
