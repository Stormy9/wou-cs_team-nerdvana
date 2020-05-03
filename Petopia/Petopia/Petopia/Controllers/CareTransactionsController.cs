using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.DAL;
using Petopia.Models;
using Petopia.Models.ViewModels;
using System.Configuration;

namespace Petopia.Controllers
{
    public class CareTransactionsController : Controller
    {
        // pull in the db through DAL\context
        private PetopiaContext db = new PetopiaContext();

        //===============================================================================
        // GET: CareTransactions
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //---------------------------------------------------------------------------
            // thank you Corrin!   [=
            //---------------------------------------------------------------------------
            CareTransactionViewModel Vmodel = new CareTransactionViewModel();

            Vmodel.IndexInfoList = (from ct in db.CareTransactions
                                    join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                                    join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                                    join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                                    join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                                    join p in db.Pets on ct.PetID equals p.PetID
                                    select new CareTransactionViewModel.IndexInfo
                                    {
                                        PetName = p.PetName,
                                        PetProviderFirstName = puP.FirstName,
                                        PetProviderLastName = puP.LastName,
                                        PetOwnerFirstName = puO.FirstName,
                                        PetOwnerLastName = puO.LastName,
                                        StartDate = ct.StartDate,
                                        EndDate = ct.EndDate,
                                        TransactionID = ct.TransactionID
                                    }).ToList();

            return View(Vmodel);
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

            //---------------------------------------------------------------------------
            // trying to pull the Pet's Name for display!         it worked!   =]
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName)
                                     .FirstOrDefault();

            ViewBag.PetName = thisPetName;

            //---------------------------------------------------------------------------
            // getting the Pet Carer name for display!              it worked!   =]
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID)
                                                  .FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName)
                                                    .FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.LastName)
                                                    .FirstOrDefault();

            ViewBag.CarerFirstName = thisCarerFirstName;
            ViewBag.CarerLastName = thisCarerLastName;

            //---------------------------------------------------------------------------
            // getting the Pet Owner name for display
            var thisPetOwnerID = careTransaction.PetOwnerID;

            var thisPetOwnerUserID = db.PetOwners.Where(po => po.PetOwnerID == thisPetOwnerID)
                                                 .Select(poID => poID.UserID)
                                                 .FirstOrDefault();

            var thisPetOwnerFirstName = db.PetopiaUsers.Where(po => po.UserID == thisPetOwnerUserID)
                                                       .Select(pon => pon.FirstName)
                                                       .FirstOrDefault();

            var thisPetOwnerLastName = db.PetopiaUsers.Where(po => po.UserID == thisPetOwnerUserID)
                                                      .Select(pon => pon.LastName)
                                                      .FirstOrDefault();

            ViewBag.PetOwnerFirstName = thisPetOwnerFirstName;
            ViewBag.PetOwnerLastName = thisPetOwnerLastName;

            //---------------------------------------------------------------------------
            // getting start & end dates -- to format the display           it worked!
            var thisStartDate = careTransaction.StartDate;
            var thisEndDate = careTransaction.EndDate;

            var formatStartDate = thisStartDate.ToString("MM/dd/yyyy");
            var formatEndDate = thisEndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = formatStartDate;
            ViewBag.ApptEndDate = formatEndDate;

            //---------------------------------------------------------------------------
            // getting start & end times -- to format the display
            var thisStartTime = careTransaction.StartDate;
            var thisEndTime = careTransaction.EndDate;

            var formatStartTime = thisStartTime.ToShortTimeString();
            var formatEndTime = thisEndTime.ToShortTimeString();

            ViewBag.ApptStartTime = formatStartTime;
            ViewBag.ApptEndTime = formatEndTime;

            //---------------------------------------------------------------------------

            return View(careTransaction);
        }
        //===============================================================================
        // GET: CareTransactions/Create
        public ActionResult BookAppointment()
        {
            // got logged-in user's ID, into the 'PetOwnerID' field, 
            //    for when a user clicks to book a Pet Care appointment!
            //
            var identityID = User.Identity.GetUserId();

            var petopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                               .Select(u => u.UserID)
                                               .FirstOrDefault();

            int petOwnerID = db.PetOwners.Where(po => po.UserID == petopiaUserID)
                                         .Select(po => po.PetOwnerID)
                                         .FirstOrDefault();

            var petOwner_UserID = db.PetOwners.Where(po => po.UserID == petopiaUserID)
                                              .Select(po => po.UserID)
                                              .FirstOrDefault();

            // for testing \ proofing.....
            ViewBag.identityID = identityID;
            ViewBag.petopiaUserID = petopiaUserID;
            ViewBag.petOwnerID = petOwnerID;
            ViewBag.petOwner_UserID = petOwner_UserID;
            
            //---------------------------------------------------------------------------
            // *** Trying to get a list of logged-in user's pets for drop-down
            //           when owner is booking a pet care appointment
            //
            //var thesePets = db.Pets.Where(po => po.PetOwnerID == loggedID);
            
            //ViewBag.UsersPets = new SelectList(db.Pets.Where(p => p.PetOwnerID = thesePets), "PetId", "PetName");

            //---------------------------------------------------------------------------

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
                var identityID = User.Identity.GetUserId();

                var petopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                              .Select(u => u.UserID)
                                              .FirstOrDefault();

                int petOwnerID = db.PetOwners.Where(po => po.UserID == petopiaUserID)
                                             .Select(po => po.PetOwnerID)
                                             .FirstOrDefault();

                // this seems to be really important, haha
                careTransaction.PetOwnerID = petOwnerID;

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
            // trying to pull the Pet's Name for display!               it worked!   =]
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName)
                                     .FirstOrDefault();

            ViewBag.PetName = thisPetName;
            //---------------------------------------------------------------------------
            // getting the Pet Owner name for display!                  it worked!   =]
            var thisOwnerID = careTransaction.PetOwnerID;

            var thisOwnerUserID = db.PetOwners.Where(cp => cp.PetOwnerID == thisOwnerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisOwnerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                    .Select(cpn => cpn.FirstName)
                                                    .FirstOrDefault();

            var thisOwnerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                   .Select(cpn => cpn.LastName)
                                                   .FirstOrDefault();

            ViewBag.OwnerFirstName = thisOwnerFirstName;
            ViewBag.OwnerLastName = thisOwnerLastName;
            //---------------------------------------------------------------------------
            // getting the Pet Carer name for display!                   it worked!   =]
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName)
                                                    .FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                   .Select(cpn => cpn.LastName)
                                                   .FirstOrDefault();

            var thisCarerAspIdentity = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                      .Select(asp => asp.ASPNetIdentityID)
                                                      .FirstOrDefault();

            var thisCarerEmail = db.ASPNetUsers.Where(pu => pu.Id == thisCarerAspIdentity)
                                               .Select(ce => ce.Email)
                                               .FirstOrDefault();                                                                                       
            ViewBag.CarerFirstName = thisCarerFirstName;
            ViewBag.CarerLastName = thisCarerLastName;
            ViewBag.CarerEmail = thisCarerEmail;
            //---------------------------------------------------------------------------
            // getting start & end dates -- to format the display           it worked!
            var thisStartDate = careTransaction.StartDate;
            var thisEndDate = careTransaction.EndDate;

            var formatStartDate = thisStartDate.ToString("MM/dd/yyyy");
            var formatEndDate = thisEndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = formatStartDate;
            ViewBag.ApptEndDate = formatEndDate;

            //---------------------------------------------------------------------------
            // getting start & end times -- to format the display
            var thisStartTime = careTransaction.StartDate;
            var thisEndTime = careTransaction.EndDate;

            var formatStartTime = thisStartTime.ToShortTimeString();
            var formatEndTime = thisEndTime.ToShortTimeString();


            ViewBag.ApptStartTime = formatStartTime;
            ViewBag.ApptEndTime = formatEndTime;

            //---------------------------------------------------------------------------

            try
            {
                var EmailSubject = "[Petopia] Pet Owner has scheduled an appointment with you";
                var EmailBody = "Hi someone has scheduled an appointment for your services, please navigate over to http://petopia-dev.azurewebsites.net to confirm the information on the request.";

                MailAddress FromEmail = new MailAddress(ConfigurationManager.AppSettings["gmailAccount"]);
                MailAddress ToEmail = new MailAddress(thisCarerEmail);

                MailMessage mail = new MailMessage(FromEmail, ToEmail);

                mail.Subject = EmailSubject;
                mail.Body = EmailBody;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["gmailAccount"], ConfigurationManager.AppSettings["gmailPassword"]);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch(Exception e)
            {

            }

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
            // pulling the Pet's Name for display!                          it worked!
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName)
                                     .FirstOrDefault();

            ViewBag.PetName = thisPetName;

            //---------------------------------------------------------------------------
            // getting the Pet Owner name for display!                  it worked!   =]
            var thisOwnerID = careTransaction.PetOwnerID;

            var thisOwnerUserID = db.PetOwners.Where(cp => cp.PetOwnerID == thisOwnerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisOwnerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                    .Select(cpn => cpn.FirstName)
                                                    .FirstOrDefault();

            var thisOwnerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                   .Select(cpn => cpn.LastName)
                                                   .FirstOrDefault();

            ViewBag.OwnerFirstName = thisOwnerFirstName;
            ViewBag.OwnerLastName = thisOwnerLastName;

            //---------------------------------------------------------------------------
            // getting the Pet Carer name for display!                      it worked!
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID)
                                                  .FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName)
                                                    .FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.LastName)
                                                    .FirstOrDefault();

            ViewBag.CarerFirstName = thisCarerFirstName;
            ViewBag.CarerLastName = thisCarerLastName;
            ViewBag.PetCarers = new SelectList(db.PetopiaUsers.OrderBy(c => c.LastName),
                                                "CarerID", "CarerName", careTransaction.CareProviderID);

            //---------------------------------------------------------------------------
            // getting start & end dates -- to format the display            it worked!
            var thisStartDate = careTransaction.StartDate;
            var thisEndDate = careTransaction.EndDate;

            var formatStartDate = thisStartDate.ToString("MM/dd/yyyy");
            var formatEndDate = thisEndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = formatStartDate;
            ViewBag.ApptEndDate = formatEndDate;

            //---------------------------------------------------------------------------
            // getting start & end times -- to format the display
            var thisStartTime = careTransaction.StartDate;
            var thisEndTime = careTransaction.EndDate;

            var formatStartTime = thisStartTime.ToString("hh:mm:tt");
            var formatEndTime = thisEndTime.ToString("hh:mm:tt");

            ViewBag.ApptStartTime = formatStartTime;
            ViewBag.ApptEndTime = formatEndTime;

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
            var identityID = User.Identity.GetUserId();

            // the logged-in user
            var petopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                               .Select(u => u.UserID)
                                               .FirstOrDefault();

            // getting the FK column 'UserID' in the 'PetOwners' table
            var petOwnerID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
                                         .Select(po => po.PetOwnerID)
                                         .FirstOrDefault();

            // this is ONLY for double-checking crap   [=
            var petOwner_UserID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
                                              .Select(po => po.UserID)
                                              .FirstOrDefault();
            // still just checking
            var user_Email = db.ASPNetUsers.Where(u => u.Id == identityID)
                                           .Select(ue => ue.Email)
                                           .FirstOrDefault();

            // this Pet Owner (instead of 'userAppts' like in 'MyAppointments')(and not a list)
            var thisPetOwner = db.CareTransactions.Where(ct => ct.PetOwnerID == petOwnerID)
                                                  .Select(tpo => tpo.PetOwnerID)
                                                  .FirstOrDefault();

            // for testing/proofing stuff!
            ViewBag.identityID = identityID;
            ViewBag.petopiaUserID = petopiaUserID;
            ViewBag.petOwnerID = petOwnerID;
            ViewBag.petOwner_UserID = petOwner_UserID;
            ViewBag.user_Email = user_Email;
            ViewBag.thisPetOwner = thisPetOwner;

            //---------------------------------------------------------------------------
            CareTransactionViewModel Vmodel = new CareTransactionViewModel();

            Vmodel.ApptInfoList = (from ct in db.CareTransactions
                                   where ct.PetOwnerID == thisPetOwner
                                   orderby ct.StartDate

                                   join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                                   join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                                   join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                                   join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                                   join p in db.Pets on ct.PetID equals p.PetID

                                   select new CareTransactionViewModel.ApptInfo
                                   {
                                       PetName = p.PetName,
                                       PetOwnerFirstName = puO.FirstName,
                                       PetOwnerLastName = puO.LastName,

                                       PetCarerFirstName = puP.FirstName,
                                       PetCarerLastName = puP.LastName,

                                       StartDate = ct.StartDate,
                                       EndDate = ct.EndDate,

                                       StartTime = ct.StartTime,
                                       EndTime = ct.EndTime,

                                       NeededThisVisit = ct.NeededThisVisit,
                                       CareProvided = ct.CareProvided,
                                       CareReport = ct.CareReport,

                                       Charge = ct.Charge,
                                       Tip = ct.Tip,

                                       PC_Rating = ct.PC_Rating,
                                       PC_Comments = ct.PC_Comments,
                                       PO_Rating = ct.PO_Rating,
                                       PO_Comments = ct.PO_Comments,

                                       PetID = ct.PetID,
                                       PetOwnerID = ct.PetOwnerID,
                                       PetCarerID = ct.CareProviderID,
                                       CareTransactionID = ct.TransactionID
                                   }).ToList();
            
            return View(Vmodel);
        }
        //===============================================================================
        // GET: CareTransactions/MyPetsAppointments/5
        public ActionResult MyPetsAppointments(int? id)
        {
            var thisPet = db.CareTransactions.Where(ct => ct.PetID == id)
                                             .Select(pID => pID.PetID)
                                             .FirstOrDefault();

            //---------------------------------------------------------------------------
            CareTransactionViewModel Vmodel = new CareTransactionViewModel();

            Vmodel.ApptInfoList = (from ct in db.CareTransactions
                                   where ct.PetID == thisPet
                                   orderby ct.StartDate

                                   join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                                   join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                                   join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                                   join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                                   join p in db.Pets on ct.PetID equals p.PetID

                                   select new CareTransactionViewModel.ApptInfo
                                   {
                                       PetName = p.PetName,
                                       PetOwnerFirstName = puO.FirstName,
                                       PetOwnerLastName = puO.LastName,

                                       PetCarerFirstName = puP.FirstName,
                                       PetCarerLastName = puP.LastName,

                                       StartDate = ct.StartDate,
                                       EndDate = ct.EndDate,

                                       StartTime = ct.StartTime,
                                       EndTime = ct.EndTime,

                                       NeededThisVisit = ct.NeededThisVisit,
                                       CareProvided = ct.CareProvided,
                                       CareReport = ct.CareReport,

                                       Charge = ct.Charge,
                                       Tip = ct.Tip,

                                       PC_Rating = ct.PC_Rating,
                                       PC_Comments = ct.PC_Comments,
                                       PO_Rating = ct.PO_Rating,
                                       PO_Comments = ct.PO_Comments,

                                       PetID = ct.PetID,
                                       PetOwnerID = ct.PetOwnerID,
                                       PetCarerID = ct.CareProviderID,
                                       CareTransactionID = ct.TransactionID
                                   }).ToList();

            return View(Vmodel);
        }
        //===============================================================================
        //===============================================================================
        public ActionResult test_crap()
        {
            var identityID = User.Identity.GetUserId();

            // the currently logged-in user
            var petopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                               .Select(u => u.UserID)
                                               .FirstOrDefault();

            // getting the FK column 'UserID' in the 'PetOwners' table
            var petOwnerID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
                                         .Select(po => po.PetOwnerID)
                                         .FirstOrDefault();

            // this is ONLY for double-checking crap   [=
            var petOwner_UserID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
                                              .Select(po => po.UserID)
                                              .FirstOrDefault();
            // still just checking
            var user_Email = db.ASPNetUsers.Where(u => u.Id == identityID)
                                           .Select(ue => ue.Email)
                                           .FirstOrDefault();

            // this Pet Owner (instead of 'userAppts' like in 'MyAppointments')(and not a list)
            var thisPetOwner = db.CareTransactions.Where(ct => ct.PetOwnerID == petOwnerID)
                                                  .Select(tpo => tpo.PetOwnerID)
                                                  .FirstOrDefault();

            // this Pet Owner's Pets -- by name (i hope, haha) -- nope..... need to do more   [=
            var thisOwnersPets = db.Pets.Where(p => p.PetOwnerID == petOwnerID)
                                        .Select(pn => pn.PetName )
                                        .ToList();

            // for testing/proofing stuff!
            ViewBag.identityID = "this user's IdentityID: " + identityID;
            ViewBag.petopiaUserID = "this user's PetopiaUserID: " + petopiaUserID;
            ViewBag.petOwnerID = "this user's PetOwnerID: " + petOwnerID;
            ViewBag.petOwner_UserID = "this user's PetOwnerID=>PetopiaUserID: " + petOwner_UserID;
            ViewBag.user_Email = "this user's email: " + user_Email;
            ViewBag.thisPetOwner = "this user's CareTransaction=>PetOwnerID: " + thisPetOwner;

            //---------------------------------------------------------------------------
            // trying to get a (displayable) list of this Owner's Pet's Names!
            //    it works btw -- into an unordered list!
            CareTransactionViewModel testLists = new CareTransactionViewModel();

            testLists.PetNameList = (from pn in db.Pets
                                             where pn.PetOwnerID == thisPetOwner
                                             select new CareTransactionViewModel.PetNames
                                             {
                                                 PetID = pn.PetID,
                                                 PetName = pn.PetName
                                             }).ToList();

            // Pet Owner Zipcode
            var petOwnerZip = db.PetopiaUsers.Where(poz => poz.UserID == petOwner_UserID)
                                             .Select(poz => poz.ResZipcode)
                                             .FirstOrDefault();

            // get list of Pet Care Provider IDs
            testLists.PetCarerList = (from cp in db.CareProviders
                                      join pu in db.PetopiaUsers on cp.UserID equals pu.UserID
                                      // left \ right side of 'equals' is important!
                                      select new CareTransactionViewModel.CareProviderInfo
                                      {
                                          CareProviderID = cp.CareProviderID,
                                          // can't believe this was this easy!  haha!
                                          CP_Name = pu.FirstName + " " + pu.LastName,
                                          CP_Zipcode = pu.ResZipcode
                                      }).ToList();

            //Pet Carers with matching Zipcode to Pet Owner
            //var petCarerZip = db.PetopiaUsers.Where(pcz => pcz.UserID == db.CareProviders.UserID)
            //                                 .

            //testLists.PetCarerList = (from cpn in db.CareProviders
            //                                   where cpn. )

            return View(testLists);
        }                                                // modeled after Edit() [GET]
        //===============================================================================
        [HttpPost]                                      // modeled after Edit() [POST]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult test_crap(int? id)
        {
            if (ModelState.IsValid)
            {
                var identityID = User.Identity.GetUserId();

                // the currently logged-in user
                var PetopiaUserID = db.PetopiaUsers.Where(pu => pu.ASPNetIdentityID == identityID)
                                                   .Select(pu => pu.UserID).First();


            }

            return View();
        }
        //===============================================================================
    }
}
