﻿using System;
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

            var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                          .Select(u => u.UserID).FirstOrDefault();

            int petOwnerID = db.PetOwners.Where(po => po.PetOwnerID == loggedID)
                                         .Select(po => po.PetOwnerID)
                                         .FirstOrDefault();

            // OR -- does this go in the [HttpPost] BookAppointment() ActionResult? [NO!]
            // seems like it makes more sense to go here?   [IT DOES!]
            //      but it's not here in 'AddPet()'... ?

            // based from Victoria's example ..... and this works!
            //   'BookAppointment()' is now bound to the currently logged-in user!
            //      why didn't it work like Corrin's AddPet() though?
            //
            ViewBag.petOwnerID = petOwnerID;

            
            //---------------------------------------------------------------------------
            // *** Trying to get a list of logged-in user's pets for drop-down
            //           when owner is booking a pet care appointment
            //
            var thesePets = db.Pets.Where(po => po.PetOwnerID == loggedID);
            
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

                var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                              .Select(u => u.UserID).FirstOrDefault();

                int petOwnerID = db.PetOwners.Where(po => po.UserID == loggedID)
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

            var thisCarerEmail = db.ASPNetUsers.Where(pu => pu.Id == thisCarerAspIdentity).Select(ce => ce.Email).FirstOrDefault();                                                                                       
                                                                                                   
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
            // getting the Pet Carer name for display!                      it worked!
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

            var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                          .Select(u => u.UserID).FirstOrDefault();


            var userAppts = db.CareTransactions.Where(ct => ct.PetOwnerID == loggedID)
                                               .OrderBy(ct => ct.StartDate);


            //---------------------------------------------------------------------------
            //
            // NEED TO PULL IN PET NAME, CARE PROVIDER FIRST + LAST NAME SOMEHOW
            //        PLUS formatted date & formatted time while we're at it
            // MADE CareTransactionViewModel BUT CAN'T GET IT PULLED IN CORRECTLY   ]=
            //
            //---------------------------------------------------------------------------


            return View(userAppts.ToList());

        }
        //===============================================================================
        // GET: CareTransactions/MyPetsAppointments/5
        public ActionResult MyPetsAppointments(int? id)
        {
            var thisPet = db.CareTransactions.Where(ct => ct.PetID == id)
                                             .OrderBy(ct => ct.StartDate);


            //---------------------------------------------------------------------------
            //
            // NEED TO PULL IN PET NAME, CARE PROVIDER FIRST + LAST NAME SOMEHOW
            //        PLUS formatted date & formatted time while we're at it
            // MADE CareTransactionViewModel BUT CAN'T GET IT PULLED IN CORRECTLY   ]=
            //
            //---------------------------------------------------------------------------


            return View(thisPet.ToList());

        }
        //===============================================================================
        public ActionResult TestingPage()
        {
            var identityID = User.Identity.GetUserId();

            var loggedID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                          .Select(u => u.UserID).FirstOrDefault();


            var userAppts = db.CareTransactions.Where(ct => ct.PetOwnerID == loggedID)
                                               .OrderBy(ct => ct.StartDate);

           
            return View(userAppts.ToList());
        }
        //===============================================================================
    }
}
