using Microsoft.AspNet.Identity;
using Petopia.DAL;
using Petopia.Models;
using Petopia.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Petopia.Controllers
{
    public class CareTransactionsController : Controller
    {
        // pull in the db through DAL\context
        private PetopiaContext db = new PetopiaContext();

        //===============================================================================
        //                                                               Appts_AdminIndex
        //===============================================================================
        // GET: CareTransactions
        [Authorize(Roles = "Admin")]
        public ActionResult Appts_AdminIndex()
        {
            //---------------------------------------------------------
            // thank you Corrin!   [=
            //---------------------------------------------------------
            CareTransactionViewModel Vmodel = new CareTransactionViewModel();

            Vmodel.IndexInfoList = (from ct in db.CareTransactions
                join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                join p in db.Pets on ct.PetID equals p.PetID
                select new CareTransactionViewModel.IndexInfo
                {
                    PetID = p.PetID, PetName = p.PetName,
                    PetOwnerID = po.PetOwnerID, 
                    PetOwnerName = puO.FirstName + " " + puO.LastName,
                    PetProviderID = cp.CareProviderID,
                    PetProviderName = puP.FirstName + " " + puP.LastName,
                    StartDate = ct.StartDate,
                    EndDate = ct.EndDate,
                    TransactionID = ct.TransactionID
                }).ToList();

            return View(Vmodel);
            // like what the admins would see -- every appointment for every user
        }
        //-------------------------------------------------------------------------------
        //                                                       also primarily for admin
        //-------------------------------------------------------------------------------
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

            //---------------------------------------------------------
            // trying to pull the Pet's Name for display!
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = thisPetName;

            //---------------------------------------------------------
            // getting the Pet Carer name for display!
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.LastName).FirstOrDefault();

            ViewBag.PetCarerName = thisCarerFirstName + " " + thisCarerLastName;

            //---------------------------------------------------------
            // getting the Pet Owner name for display
            var thisPetOwnerID = careTransaction.PetOwnerID;

            var thisPetOwnerUserID = db.PetOwners.Where(po => po.PetOwnerID == thisPetOwnerID)
                                                 .Select(poID => poID.UserID).FirstOrDefault();

            var thisPetOwnerFirstName = db.PetopiaUsers.Where(po => po.UserID == thisPetOwnerUserID)
                                                       .Select(pon => pon.FirstName).FirstOrDefault();

            var thisPetOwnerLastName = db.PetopiaUsers.Where(po => po.UserID == thisPetOwnerUserID)
                                                      .Select(pon => pon.LastName).FirstOrDefault();

            ViewBag.PetOwnerName = thisPetOwnerFirstName + " " + thisPetOwnerLastName;

            //---------------------------------------------------------
            // getting start & end dates -- to format the display
            var thisStartDate = careTransaction.StartDate.ToString("MM/dd/yyyy");
            var thisEndDate = careTransaction.EndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = thisStartDate;
            ViewBag.ApptEndDate = thisEndDate;

            //---------------------------------------------------------

            return View(careTransaction);
        }
        //===============================================================================
        //                                                            Appointment BOOKING
        //===============================================================================
        // GET: CareTransactions/BookAppointment/5
        public ActionResult BookAppointment(int? id)
        {
            //---------------------------------------------------------
            // get the pet that was passed in:
            var thisPetID = id;
            ViewBag.thisPetId = thisPetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.thisPetName = thisPetName;
            //---------------------------------------------------------
            // get logged-in user's PetOwnerID, to put into the 'PetOwnerID' field:
            var identityID = User.Identity.GetUserId();

            var thisPetopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                                   .Select(u => u.UserID).FirstOrDefault();

            int thisPetOwnerID = db.PetOwners.Where(po => po.UserID == thisPetopiaUserID)
                                             .Select(po => po.PetOwnerID).FirstOrDefault();

            ViewBag.petOwnerID = thisPetOwnerID;

            // this logged-in PetopiaUser(PetOwner)'s Zipcode
            var thisPetOwnerZip = db.PetopiaUsers.Where(poz => poz.UserID == thisPetopiaUserID)
                                             .Select(poz => poz.ResZipcode).FirstOrDefault();

            //                                                         still in Book[GET]
            //---------------------------------------------------------------------------
            // to make sure only the pet's owner can see this page!
            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();

            var loggedInUser = User.Identity.GetUserId();


            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;

            //---------------------------------------------------------
            // see 'test_crap' at the end here for notes..... 
            // --> SELECT LIST OF THIS LOGGED-IN PET OWNER'S PETS <--
            List<SelectListItem> ThisOwnersPetsSelectList = (from pn in db.Pets
                                                    where pn.PetOwnerID == thisPetOwnerID
                                                    select new SelectListItem
                                                    {
                                                        Value = pn.PetID.ToString(),
                                                        Text = pn.PetName

                                                    }).ToList();

            //ViewBag.ThisOwnersPetsSelectList = new SelectList(ThisOwnersPetsSelectList);
            // ^^^ for one thing, the drop-down says 'System.Web.Mvc.SelectListItem'
            // error returned:  System.InvalidOperationException: 
            //                  There is no ViewData item of type 'IEnumerable<SelectListItem>' 
            //                  that has the key 'ThisOwnersPetsSelectList'.
            //
            // there's also the:  `new SelectList(PetCarerSelectList, "value", "text")`
            // similar to how we did for HW8 in 460 -- but wtf to put for those, w/query?
            //    -->  page won't even load w/the things i tried for "value" & "text"
            //
            //   --> comment one or the other out to see what it does <--
            //
            // this one actually shows the pet names!  but returns error:
            //          System.InvalidOperationException: 
            //          There is no ViewData item of type 'IEnumerable<SelectListItem>' 
            //          that has the key 'ThisOwnersPetsSelectList'.
            ViewBag.ThisOwnersPetsSelectList = ThisOwnersPetsSelectList;

            //---------------------------------------------------------------------------
            // --> SELECT LIST OF PET CARERS w/ZIPCODE MATCHING THIS LOGGED-IN PET OWNER
            List<SelectListItem> PetCarerSelectList = (from pu in db.PetopiaUsers
                                where pu.ResZipcode == thisPetOwnerZip
                                join cp in db.CareProviders on pu.UserID equals cp.UserID
                                select new SelectListItem
                                {
                                    Value = cp.CareProviderID.ToString(),
                                    Text = pu.FirstName + " " + pu.LastName

                                }).ToList();

            //ViewBag.PetCarerSelectList = new SelectList(PetCarerSelectList);
            //
            //   --> comment one or the other out to see what it does <--
            //
            // essentially exactly the same behavior as with the pet list .....
            ViewBag.PetCarerSelectList = PetCarerSelectList;

            // tried `ViewData` again after Joey said it -- it still confuses me, this:
            //ViewData.PetCarerSelectList = PetCarerSelectList;
            // ^-- this makes red squiggly w/error message.....

            //                                                         still in Book[GET]
            //---------------------------------------------------------------------------
            CareTransactionViewModel MatchingPetCarers = new CareTransactionViewModel();

            // get list of Pet Care Provider IDs + Names--
            //  where CareProvider Zipcode == currentlyLogged - inUser Zipcode-- it works!
            //    --> THIS IS WHAT MAKES THE BLUE CARDS ON THE 'test_crap' VIEW < --
            MatchingPetCarers.PetCarerList = (from pu in db.PetopiaUsers
                                where pu.ResZipcode == thisPetOwnerZip
                                join cp in db.CareProviders on pu.UserID equals cp.UserID
                                select new CareTransactionViewModel.CareProviderInfo
                                {
                                    CareProviderID = cp.CareProviderID,
                                    CP_Name = pu.FirstName + " " + pu.LastName,

                                }).ToList();

            //---------------------------------------------------------

            return View(MatchingPetCarers);
        }
        //-------------------------------------------------------------------------------
        //                                                               still in booking
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/BookAppointment
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport," +
          "Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID," +
          "CareProviderID,PetID,NeededThisVisit,Pending,Confirmed,Completed_PO,Completed_CP")] 
                                                            CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                // get logged-in user's PetOwnerID, into the 'PetOwnerID' field,
                var identityID = User.Identity.GetUserId();

                var thisPetopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                                       .Select(u => u.UserID).FirstOrDefault();

                int thisPetOwnerID = db.PetOwners.Where(po => po.UserID == thisPetopiaUserID)
                                                 .Select(po => po.PetOwnerID).FirstOrDefault();

                // this logged-in PetopiaUser(PetOwner)'s Zipcode
                var thisPetOwnerZip = db.PetopiaUsers.Where(poz => poz.UserID == thisPetopiaUserID)
                                                     .Select(poz => poz.ResZipcode).FirstOrDefault();
                //-----------------------------------------------------

                // this seems to be really important, haha
                careTransaction.PetOwnerID = thisPetOwnerID;

                // and let's add this:
                careTransaction.Pending = true;
                careTransaction.Confirmed = false;
                careTransaction.Completed_PO = false;
                careTransaction.Completed_CP = false;

                //-----------------------------------------------------
                // to make sure only the pet's owner can see this page!

                var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers
                        .Where(pu => pu.UserID == thisPetopiaUserID)
                        .Select(aspnetID => aspnetID.ASPNetIdentityID).FirstOrDefault();

                var loggedInUser = User.Identity.GetUserId();

                ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
                ViewBag.loggedInUser = loggedInUser;                
                //-----------------------------------------------------

                db.CareTransactions.Add(careTransaction);

                db.SaveChanges();

                return RedirectToAction("BookingConfirmation", 
                                         new { id = careTransaction.TransactionID });
            }
            //---------------------------------------------------------
            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                                                               still in booking
        //-------------------------------------------------------------------------------
        // GET: CareTransactions/BookingConfirmation/5
        public ActionResult BookingConfirmation(int? id)
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
            //---------------------------------------------------------
            // trying to pull the Pet's Name for display!
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = thisPetName;
            //---------------------------------------------------------
            // getting the Pet Owner name for display!
            var thisOwnerID = careTransaction.PetOwnerID;

            var thisOwnerUserID = db.PetOwners.Where(cp => cp.PetOwnerID == thisOwnerID)
                                              .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisOwnerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisOwnerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                   .Select(cpn => cpn.LastName).FirstOrDefault();

            var thisOwnerAspIdentity = db.PetopiaUsers.Where(po => po.UserID == thisOwnerUserID)
                                                      .Select(asp => asp.ASPNetIdentityID).FirstOrDefault();

            var thisOwnerEmail = db.ASPNetUsers.Where(pu => pu.Id == thisOwnerAspIdentity)
                                               .Select(pe => pe.Email).FirstOrDefault();

            ViewBag.PetOwnerName = thisOwnerFirstName + " " + thisOwnerLastName;

            //---------------------------------------------------------
            // getting the Pet Carer name for display!
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                   .Select(cpn => cpn.LastName).FirstOrDefault();

            var thisCarerAspIdentity = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                      .Select(asp => asp.ASPNetIdentityID).FirstOrDefault();

            var thisCarerEmail = db.ASPNetUsers.Where(pu => pu.Id == thisCarerAspIdentity)
                                               .Select(ce => ce.Email).FirstOrDefault();

            ViewBag.PetcarerName = thisCarerFirstName + " " + thisCarerLastName;
            ViewBag.CarerEmail = thisCarerEmail;

            //---------------------------------------------------------
            // getting start & end dates -- to format the display
            var thisStartDate = careTransaction.StartDate.ToString("MM/dd/yyyy");
            var thisEndDate = careTransaction.EndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = thisStartDate;
            ViewBag.ApptEndDate = thisEndDate;
            //                                      THE EMAIL STUFF GOES IN CONFIRMATIONS
            //---------------------------------------------------------------------------
            // SEND EMAILS TO (OWNER &) CARER                         BookingConfirmation
            try
            {
                var EmailSubject_to_Carer = "[Petopia] Pet Owner has scheduled an appointment with you";
                var EmailBody_to_Carer = "Hi! A Petopia User has scheduled an appointment for your services, " +
                    "please navigate over to http://petopia.azurewebsites.net to confirm the pet care request. ";

                var EmailSubject_to_Owner = "[Petopia] Your Pet Care Appointment Booking Confirmation";
                var EmailBody_to_Owner = "Thank you for booking a pet care appointment through Petopia!" +
                    "Your requested Pet Care Provider has been notified.  You will receive another email " +
                    "when they confirm your request.";

                MailAddress FromEmail = new MailAddress(ConfigurationManager.AppSettings["gmailAccount"]);
                MailAddress ToEmail_Carer = new MailAddress(thisCarerEmail);
                MailAddress ToEmail_Owner = new MailAddress(thisOwnerEmail);

                MailMessage mail_to_carer = new MailMessage(FromEmail, ToEmail_Carer);
                MailMessage mail_to_owner = new MailMessage(FromEmail, ToEmail_Owner);

                mail_to_carer.Subject = EmailSubject_to_Carer;
                mail_to_carer.Body = EmailBody_to_Carer;

                mail_to_owner.Subject = EmailSubject_to_Owner;
                mail_to_owner.Body = EmailBody_to_Owner;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["gmailAccount"], ConfigurationManager.AppSettings["gmailPassword"]);
                smtp.EnableSsl = true;
                smtp.Send(mail_to_carer);
            }
            catch (Exception e)
            {
            }

            //---------------------------------------------------------
            return View(careTransaction);
        }
        //===============================================================================
        //===============================================================================
        //                                                         Appointment CONFIRMING
        //===============================================================================
        // GET: CareTransactions/ConfirmAppointment/5                   base off 'Edit()'
        public ActionResult ConfirmAppointment(int? ct_id)
        {
            // for initial proofing in the view
            ViewBag.thisApptID = ct_id;

            // so first we gotta find the apppointment & pull it up -- like in 'Edit()'
            if (ct_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(ct_id);

            if (careTransaction == null)
            {
                return HttpNotFound();
            }                                                                      // GET
            //---------------------------------------------------------------------------
            // make sure that only the requested care provider can access this page
            // get logged-in user's PetOwnerID, into the 'PetOwnerID' field,
            var loggedInUser = User.Identity.GetUserId();

            var loggedInPetopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == loggedInUser)
                                                       .Select(u => u.UserID).FirstOrDefault();

            var loggedInPetCarerID = db.CareProviders.Where(po => po.UserID == loggedInPetopiaUserID)
                                                     .Select(po => po.CareProviderID).FirstOrDefault();

            ViewBag.loggedInUser = loggedInUser;
            ViewBag.loggedInPetopiaUserID = loggedInPetopiaUserID;
            ViewBag.loggedInPetCarerID = loggedInPetCarerID;
            //---------------------------------------------------------
            var reqPetCarerID = db.CareTransactions.Where(cp => cp.TransactionID == ct_id)
                                                   .Select(cpID => cpID.CareProviderID).FirstOrDefault();
            // just proofing
            var reqPetCarerCP_ID = db.CareProviders.Where(cp => cp.CareProviderID == reqPetCarerID)
                                                   .Select(cp => cp.CareProviderID).FirstOrDefault();
            // proofing.....
            var reqPetCarerPU_ID = db.CareProviders.Where(pu => pu.CareProviderID == reqPetCarerCP_ID)
                                                   .Select(puID => puID.UserID).FirstOrDefault();
            // proofing.....
            var reqPetCarerASPNetID = db.PetopiaUsers.Where(pu => pu.UserID == reqPetCarerPU_ID)
                                                     .Select(aID => aID.ASPNetIdentityID).FirstOrDefault();

            ViewBag.reqPetCarerID = reqPetCarerID;
            ViewBag.reqPetCarerCP_ID = reqPetCarerCP_ID;
            ViewBag.reqPetCarerPU_ID = reqPetCarerPU_ID;
            ViewBag.reqPetCarerASPNetID = reqPetCarerASPNetID;
            //---------------------------------------------------------
            // pull in Pet & Pet Owner names & format the date better
            var thisPetID = db.CareTransactions.Where(ct => ct.TransactionID == ct_id)
                                               .Select(pID => pID.PetID)
                                               .FirstOrDefault();

            var petName = db.Pets.Where(pID => pID.PetID == thisPetID)
                                 .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = petName;

            //---------------------------------------------------------
            var thisPetOwnerID = db.CareTransactions.Where(ct => ct.TransactionID == ct_id)
                                                    .Select(poID => poID.PetOwnerID).FirstOrDefault();

            var thisPetOwnerPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisPetOwnerID)
                                                    .Select(poID => poID.UserID).FirstOrDefault();

            var thisPetOwnerFirstName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetOwnerPetopiaID)
                                                       .Select(poFN => poFN.FirstName).FirstOrDefault();

            var thisPetOwnerLastName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetOwnerPetopiaID)
                                                      .Select(poLN => poLN.LastName).FirstOrDefault();

            ViewBag.PetOwnerName = thisPetOwnerFirstName + " " + thisPetOwnerLastName;

            //---------------------------------------------------------
            var thisStartDate = db.CareTransactions.Where(ct => ct.TransactionID == ct_id)
                                                   .Select(ct => ct.StartDate)
                                                   .FirstOrDefault().ToString("MMMM dd, yyyy");

            var thisEndDate = db.CareTransactions.Where(ct => ct.TransactionID == ct_id)
                                                 .Select(ct => ct.EndDate)
                                                 .FirstOrDefault().ToString("MMMM dd, yyyy");

            ViewBag.StartDate = thisStartDate;
            ViewBag.EndDate = thisEndDate;

            //---------------------------------------------------------
            // the (careTransaction) is super-important, haha
            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                                                still in Appointment-Confirming
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/ConfirmAppointment/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport," +
          "Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID," +
          "CareProviderID,PetID,NeededThisVisit,Pending,Confirmed,Completed_PO,Completed_CP")] 
                                                        CareTransaction careTransaction)
        {
            // so why *do* some of these have all that ^^^ and others don't?

            if (ModelState.IsValid)
            {
                db.Entry(careTransaction).State = EntityState.Modified;

                careTransaction.Pending = false;
                careTransaction.Confirmed = true;

                db.SaveChanges();

                return RedirectToAction("ConfirmConfirmation",
                                        new { ct_id = careTransaction.TransactionID });
            }
            //---------------------------------------------------------
            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                                                still in Appointment-Confirming
        //-------------------------------------------------------------------------------
        public ActionResult ConfirmConfirmation(int? ct_id)    // do like booking confirm
        {
            // pull in PetOwner/PetCarer name + email, PetName, appt request recap
            if (ct_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(ct_id);

            if (careTransaction == null)
            {
                return HttpNotFound();
            }
            //---------------------------------------------------------
            // get logged-in user identity:
            var loggedInUser = User.Identity.GetUserId();
            var thisApptID = ct_id;

            ViewBag.LoggedInUser = loggedInUser;
            ViewBag.thisApptID = thisApptID;
            //---------------------------------------------------------
            // trying to pull the Pet's Name for display!   
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = thisPetName;
            //---------------------------------------------------------
            // getting the Pet Owner name & email & main phone for display!    
            var thisOwnerID = careTransaction.PetOwnerID;

            var thisOwnerPetopiaID = db.PetOwners.Where(cp => cp.PetOwnerID == thisOwnerID)
                                                 .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisOwnerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerPetopiaID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisOwnerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerPetopiaID)
                                                   .Select(cpn => cpn.LastName).FirstOrDefault();

            var thisOwnerAspIdentity = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerPetopiaID)
                                                      .Select(asp => asp.ASPNetIdentityID).FirstOrDefault();

            var thisOwnerEmail = db.ASPNetUsers.Where(pu => pu.Id == thisOwnerAspIdentity)
                                               .Select(ce => ce.Email).FirstOrDefault();

            var thisOwnerMainPhone = db.PetopiaUsers.Where(pu => pu.UserID == thisOwnerPetopiaID)
                                                    .Select(mp => mp.MainPhone).FirstOrDefault();

            var thisIsOwner = db.PetopiaUsers.Where(pu => pu.UserID == thisOwnerPetopiaID)
                                             .Select(io => io.IsOwner).FirstOrDefault();

            ViewBag.PetOwnerName = thisOwnerFirstName + " " + thisOwnerLastName;
            ViewBag.PetOwnerEmail = thisOwnerEmail;
            ViewBag.PetOwnerMainPhone = thisOwnerMainPhone;
            ViewBag.PetOwnerPetopiaID = thisOwnerPetopiaID;
            ViewBag.thisOwnerAspIdentity = thisOwnerAspIdentity;
            ViewBag.thisIsOwner = thisIsOwner;
            //---------------------------------------------------------
            // getting the Pet Carer name & email & main phone for display!           
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerPetopiaID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerPetopiaID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerPetopiaID)
                                                   .Select(cpn => cpn.LastName).FirstOrDefault();

            var thisCarerAspIdentity = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerPetopiaID)
                                                      .Select(asp => asp.ASPNetIdentityID).FirstOrDefault();

            var thisCarerEmail = db.ASPNetUsers.Where(pu => pu.Id == thisCarerAspIdentity)
                                               .Select(ce => ce.Email).FirstOrDefault();

            var thisCarerMainPhone = db.PetopiaUsers.Where(pu => pu.UserID == thisCarerPetopiaID)
                                                    .Select(mp => mp.MainPhone).FirstOrDefault();

            var thisIsCarer = db.PetopiaUsers.Where(pu => pu.UserID == thisOwnerPetopiaID)
                                             .Select(ip => ip.IsProvider).FirstOrDefault();

            ViewBag.PetCarerName = thisCarerFirstName + " " + thisCarerLastName;
            ViewBag.PetCarerEmail = thisCarerEmail;
            ViewBag.PetCarerMainPhone = thisCarerMainPhone;
            ViewBag.PetCarerPetopiaID = thisCarerPetopiaID;
            ViewBag.ThisCarerAspIdentity = thisCarerAspIdentity;
            ViewBag.thisIsCarer = thisIsCarer;
            //---------------------------------------------------------
            // getting start & end dates -- to format the display          
            var thisStartDate = careTransaction.StartDate.ToString("MMMM dd, yyyy");
            var thisEndDate = careTransaction.EndDate.ToString("MMMM dd, yyyy");

            ViewBag.ApptStartDate = thisStartDate;
            ViewBag.ApptEndDate = thisEndDate;
            //                                      THE EMAIL STUFF GOES IN CONFIRMATIONS
            //---------------------------------------------------------------------------
            // SEND EMAILS TO (OWNER &) CARER                         ConfirmConfirmation
            try
            {
                var EmailSubject_to_Carer = "[Petopia] You have confirmed your Pet Care Appointment!";
                var EmailBody_to_Carer = "Hi! You confirmed your Pet Care Appointment.  Be sure to " +
                    "mark your calendar so you don't forget -- the Pet Owner is counting on you!  " + 
                    "Please navigate over to http://petopia.azurewebsites.net to keep track of all of" +
                    "your appointments! ";

                var EmailSubject_to_Owner = "[Petopia] Your Pet Care Provider has confirmed your Appointment Request!";
                var EmailBody_to_Owner = "Hi! Your selected Pet Care Provider has confirmed your Pet " +
                    "Care Appointment request.  Please navigate over to http://petopia.azurewebsites.net " +
                    "to keep track of all of your appointments!";

                MailAddress FromEmail = new MailAddress(ConfigurationManager.AppSettings["gmailAccount"]);
                MailAddress ToEmail_Carer = new MailAddress(thisCarerEmail);
                MailAddress ToEmail_Owner = new MailAddress(thisOwnerEmail);

                MailMessage mail_to_carer = new MailMessage(FromEmail, ToEmail_Carer);
                MailMessage mail_to_owner = new MailMessage(FromEmail, ToEmail_Owner);

                mail_to_carer.Subject = EmailSubject_to_Carer;
                mail_to_carer.Body = EmailBody_to_Carer;

                mail_to_owner.Subject = EmailSubject_to_Owner;
                mail_to_owner.Body = EmailBody_to_Owner;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["gmailAccount"], ConfigurationManager.AppSettings["gmailPassword"]);
                smtp.EnableSsl = true;
                smtp.Send(mail_to_carer);
            }
            catch (Exception e)
            {
            }
            //---------------------------------------------------------

            return View(careTransaction);
        }
        //===============================================================================
        //                      once Appt is Confirmed -- this 'JobDetails' for Pet Carer
        //===============================================================================
        public ActionResult PetCarer_JobDetails(int? ct_id)
        {
            // pull up the full requested CareTransaction:
            if (ct_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction thisJob = db.CareTransactions.Find(ct_id);

            if (thisJob == null)
            {
                return HttpNotFound();
            }
            //---------------------------------------------------------
            // GET CURRENTLY LOGGED-IN USER STUFF -- for privacy issues
            //
            // the currently logged-in user:
            var identityID = User.Identity.GetUserId();

            // the currently logged-in user's PetopiaUserID:
            var thisPetopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                                   .Select(u => u.UserID).FirstOrDefault();

            ViewBag.LoggedInPetopiaUserID = thisPetopiaUserID;
            //---------------------------------------------------------
            // GET THIS TRANSACTION ID -- for proofing
            //
            var thisJobID = thisJob.TransactionID;

            ViewBag.ThisJobID = thisJobID;
            //---------------------------------------------------------
            // GET THS CARE PROVIDER ON *THIS* CareTransaction:
            //
            // get CP_ID outta the CareTransactionID passed in
            var thisPetCarerID = thisJob.CareProviderID;

            // get THIS CP's PetopiaUserID
            var thisPetCarerPetopiaID = db.CareProviders.Where(cp => cp.CareProviderID == thisPetCarerID)
                                                        .Select(pu => pu.UserID).FirstOrDefault();

            ViewBag.ThisPetCarerID = thisPetCarerID;
            ViewBag.ThisPetCarerPetopiaID = thisPetCarerPetopiaID;
            //---------------------------------------------------------
            // PET OWNER ON *THIS* CareTransaction:
            //
            // get the PO_ID outta the CareTransactionID passed in
            var thisPetOwnerID = thisJob.PetOwnerID;

            // get THIS PO's PetopiaUserID
            var thisPetOwnerPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisPetOwnerID)
                                                    .Select(pu => pu.UserID).FirstOrDefault();

            var thisPetOwnerAspNetID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetOwnerPetopiaID)
                                                      .Select(aspID => aspID.ASPNetIdentityID).FirstOrDefault();

            ViewBag.ThisPetOwnerID = thisPetOwnerID;
            ViewBag.ThisPetOwnerPetopiaID = thisPetOwnerPetopiaID;
            //---------------------------------------------------------
            // get the PetID outta the CareTransactionID passed in
            var thisPetID = thisJob.PetID;

            ViewBag.ThisPetID = thisPetID;
            //---------------------------------------------------------
            // the appointment stuff:
            // format the times once you change the db data-types.....
            //
            var thisStartDate = thisJob.StartDate.ToString("MM/dd/yyyy");
            var thisStartTime = thisJob.StartTime.ToString("h:mm tt");

            var thisEndDate = thisJob.EndDate.ToString("MM/dd/yyyy");
            var thisEndTime = thisJob.EndTime.ToString("h:mm tt");

            var neededThisVisit = thisJob.NeededThisVisit;


            ViewBag.ThisStartDate = thisStartDate;
            ViewBag.ThisStartTime = thisStartTime;

            ViewBag.ThisEndDate = thisEndDate;
            ViewBag.ThisEndTime = thisEndTime;

            ViewBag.NeededThisVisit = neededThisVisit;
            //---------------------------------------------------------
            // the pet owner stuff:
            //
            var thisPetOwnerFirstName = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                       .Select(poFN => poFN.FirstName).FirstOrDefault();

            var thisPetOwnerLastName = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                      .Select(poLN => poLN.LastName).FirstOrDefault();

            ViewBag.ThisPetOwnerName = thisPetOwnerFirstName + " " + thisPetOwnerLastName;


            var thisPetOwnerAdd01 = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                   .Select(poRAdd01 => poRAdd01.ResAddress01).FirstOrDefault();

            var thisPetOwnerAdd02 = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                   .Select(poRAdd02 => poRAdd02.ResAddress02).FirstOrDefault();

            var thisPetOwnerCity = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                  .Select(poRC => poRC.ResCity).FirstOrDefault();

            var thisPetOwnerState = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                   .Select(poRS => poRS.ResState).FirstOrDefault();

            var thisPetOwnerZip = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                 .Select(poRZ => poRZ.ResZipcode).FirstOrDefault();

            ViewBag.ThisPetOwnerResAdd01 = thisPetOwnerAdd01;
            ViewBag.ThisPetOwnerResAdd02 = thisPetOwnerAdd02;
            ViewBag.ThisPetOwnerCity = thisPetOwnerCity;
            ViewBag.ThisPetOwnerState = thisPetOwnerState;
            ViewBag.ThisPetOwnerZip = thisPetOwnerZip;


            var thisPetOwnerMainPhone = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                       .Select(poMP => poMP.MainPhone).FirstOrDefault();

            var thisPetOwnerAltPhone = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                      .Select(poAP => poAP.AltPhone).FirstOrDefault();

            ViewBag.ThisPetOwnerMainPhone = thisPetOwnerMainPhone;
            ViewBag.ThisPetOwnerAltPhone = thisPetOwnerAltPhone;

            var petOwnerEmail = db.ASPNetUsers.Where(aspID => aspID.Id == thisPetOwnerAspNetID)
                                              .Select(poE => poE.Email).FirstOrDefault();

            ViewBag.petOwnerEmail = petOwnerEmail;

            var thisPetOwnerHomeAccess = db.PetOwners.Where(puID => puID.PetOwnerID == thisPetOwnerID)
                                                     .Select(poHA => poHA.HomeAccess).FirstOrDefault();

            ViewBag.ThisPetOwnerHomeAccess = thisPetOwnerHomeAccess;

            //---------------------------------------------------------
            // the pet stuff:
            var thisPetName = db.Pets.Where(petID => petID.PetID == thisPetID)
                                     .Select(petN => petN.PetName).FirstOrDefault();

            var thisPetSpecies = db.Pets.Where(petID => petID.PetID == thisPetID)
                                        .Select(petS => petS.Species).FirstOrDefault();

            var thisPetBreed = db.Pets.Where(petID => petID.PetID == thisPetID)
                                      .Select(petB => petB.Breed).FirstOrDefault();

            var thisPetGender = db.Pets.Where(petID => petID.PetID == thisPetID)
                                       .Select(petG => petG.Gender).FirstOrDefault();

            var thisPetBday = db.Pets.Where(petID => petID.PetID == thisPetID)
                                     .Select(petBday => petBday.Birthdate).FirstOrDefault();

            var thisPetWeight = db.Pets.Where(petID => petID.PetID == thisPetID)
                                       .Select(petW => petW.Weight).FirstOrDefault();

            ViewBag.ThisPetName = thisPetName;
            ViewBag.ThisPetSpecies = thisPetSpecies;
            ViewBag.ThisPetBreed = thisPetBreed;
            ViewBag.ThisPetGender = thisPetGender;
            ViewBag.ThisPetBday = thisPetBday.ToString("MMMM dd, yyyy");
            ViewBag.ThisPetWeight = thisPetWeight + " lbs.";


            var thisPetHealth = db.Pets.Where(petID => petID.PetID == thisPetID)
                                       .Select(petHC => petHC.HealthConcerns).FirstOrDefault();

            var thisPetBehavior = db.Pets.Where(petID => petID.PetID == thisPetID)
                                         .Select(petBC => petBC.BehaviorConcerns).FirstOrDefault();

            var thisPetAccess = db.Pets.Where(petID => petID.PetID == thisPetID)
                                       .Select(petA => petA.PetAccess).FirstOrDefault();

            ViewBag.ThisPetHealth = thisPetHealth;
            ViewBag.ThisPetBehavior = thisPetBehavior;
            ViewBag.ThisPetAccess = thisPetAccess;


            var thisPetECName = db.Pets.Where(petID => petID.PetID == thisPetID)
                                       .Select(petECN => petECN.EmergencyContactName).FirstOrDefault();

            var thisPetECPhone = db.Pets.Where(petID => petID.PetID == thisPetID)
                                        .Select(petECP => petECP.EmergencyContactPhone).FirstOrDefault();

            ViewBag.ThisPetECName = thisPetECName;
            ViewBag.ThisPetECPhone = thisPetECPhone;

            //---------------------------------------------------------

            return View(thisJob);
        }
        //===============================================================================
        //                                                            Appointment EDITING
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
            //---------------------------------------------------------
            // to make sure only the pet's owner can see this page!
            var thisPetsID = careTransaction.PetID;

            var thisPetsOwnersID = careTransaction.PetOwnerID;

            var thisPetsOwnersPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisPetsOwnersID)
                                                          .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();
            var loggedInUser = User.Identity.GetUserId();

            var petName = db.Pets.Where(p => p.PetID == thisPetsID)
                                 .Select(pn => pn.PetName).FirstOrDefault();

            var thisPetOwnerZip = db.PetopiaUsers.Where(poz => poz.UserID == thisPetsOwnersPetopiaID)
                                                     .Select(poz => poz.ResZipcode).FirstOrDefault();

            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;
            ViewBag.PetName = petName;

            //---------------------------------------------------------
            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                                                   still in Appointment-Editing
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/EditAppointment/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport,Charge," +
          "Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID,CareProviderID,PetID," +
            "NeededThisVisit,Pending,Confirmed,Completed_PO,Completed_CP")]CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(careTransaction).State = EntityState.Modified;

                careTransaction.Pending = true;
                careTransaction.Confirmed = false;

                db.SaveChanges();

                return RedirectToAction("EditConfirmation", 
                                        new { id = careTransaction.TransactionID });
            }

            //---------------------------------------------------------
            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                                                   still in Appointment-Editing
        //-------------------------------------------------------------------------------
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
            //---------------------------------------------------------
            // pulling the Pet's Name for display! 
            var thisPetID = careTransaction.PetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = thisPetName;

            //---------------------------------------------------------
            // getting the Pet Owner name for display! 
            var thisOwnerID = careTransaction.PetOwnerID;

            var thisOwnerPetopiaID = db.PetOwners.Where(cp => cp.PetOwnerID == thisOwnerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisOwnerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerPetopiaID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisOwnerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerPetopiaID)
                                                   .Select(cpn => cpn.LastName).FirstOrDefault();

            // get Pet Owner's email address:
            var thisOwnerAspIdentity = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerPetopiaID)
                                                      .Select(asp => asp.ASPNetIdentityID).FirstOrDefault();

            var thisOwnerEmail = db.ASPNetUsers.Where(pu => pu.Id == thisOwnerAspIdentity)
                                               .Select(ce => ce.Email).FirstOrDefault();


            ViewBag.PetOwnerName = thisOwnerFirstName + " " + thisOwnerLastName;
            //---------------------------------------------------------
            // getting the Pet Carer name for display! 
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerPetopiaID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerPetopiaID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerPetopiaID)
                                                    .Select(cpn => cpn.LastName).FirstOrDefault();

            // get Pet Carer's email address:
            var thisCarerAspIdentity = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerPetopiaID)
                                                      .Select(asp => asp.ASPNetIdentityID).FirstOrDefault();

            var thisCarerEmail = db.ASPNetUsers.Where(pu => pu.Id == thisOwnerAspIdentity)
                                               .Select(ce => ce.Email).FirstOrDefault();


            ViewBag.PetCarerName = thisCarerFirstName + " " + thisCarerLastName;
            //---------------------------------------------------------
            // getting start & end dates -- to format the display
            var thisStartDate = careTransaction.StartDate.ToString("MM/dd/yyyy");
            var thisEndDate = careTransaction.EndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = thisStartDate;
            ViewBag.ApptEndDate = thisEndDate;

            //                                      THE EMAIL STUFF GOES IN CONFIRMATIONS
            //---------------------------------------------------------------------------
            // SEND EMAILS TO (OWNER &) CARER                            EditConfirmation
            try
            {
                var EmailSubject_to_Carer = "[Petopia] Pet Owner has edited their appointment with you";
                var EmailBody_to_Carer = "Hi! A Petopia User has edited one of their " +
                    "appointments with you, please navigate over to " +
                    "http://petopia.azurewebsites.net  to track all of yourappointments.";

                var EmailSubject_to_Owner = "[Petopia] Your Pet Care Appointment Edit Confirmation";
                var EmailBody_to_Owner = "You requested to reschedule your Pet Care Appointment." +
                    "Your selected Pet Care Provider has been notified.  You will receive another " +
                    "email when they confirm your reschedule request.";

                MailAddress FromEmail = new MailAddress(ConfigurationManager.AppSettings["gmailAccount"]);
                MailAddress ToEmail_Carer = new MailAddress(thisCarerEmail);
                MailAddress ToEmail_Owner = new MailAddress(thisOwnerEmail);

                MailMessage mail_to_carer = new MailMessage(FromEmail, ToEmail_Carer);
                MailMessage mail_to_owner = new MailMessage(FromEmail, ToEmail_Owner);

                mail_to_carer.Subject = EmailSubject_to_Carer;
                mail_to_carer.Body = EmailBody_to_Carer;

                mail_to_owner.Subject = EmailSubject_to_Owner;
                mail_to_owner.Body = EmailBody_to_Owner;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager
                    .AppSettings["gmailAccount"], ConfigurationManager.AppSettings["gmailPassword"]);
                smtp.EnableSsl = true;
                smtp.Send(mail_to_carer);
            }
            catch (Exception e)
            {
            }
            //---------------------------------------------------------

            return View(careTransaction);
        }
        //===============================================================================
        //                                     Appointment COMPLETION -- PET OWNER -- GET
        //===============================================================================
        // GET: CareTransactions/CompleteAppointment/5
        public ActionResult CompleteAppointment_PetOwner(int? id)
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
            //---------------------------------------------------------
            // to make sure only the pet's owner & carer can see this page!
            var thisCareTransactionPetID = careTransaction.PetID;

            var thisPetsOwnersID = careTransaction.PetOwnerID;

            var thisPetsOwnersPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisPetsOwnersID)
                                                      .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsOwnersASPNetID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaID)
                                                        .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                        .FirstOrDefault();

            //---------------------------------------------------------
            var thisPetsCarersID = careTransaction.CareProviderID;

            var thisPetsCarersPetopiaID = db.CareProviders.Where(cp => cp.CareProviderID == thisPetsCarersID)
                                                          .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisPetsCarersASPNetID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaID)
                                                        .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                        .FirstOrDefault();
            //---------------------------------------------------------
            var loggedInUser = User.Identity.GetUserId();

            //---------------------------------------------------------
            var petName = db.Pets.Where(p => p.PetID == careTransaction.PetID)
                                 .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = petName;
            //---------------------------------------------------------
            var petCarerFirstName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaID)
                                                   .Select(cfn => cfn.FirstName).FirstOrDefault();
            var petCarerLastName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaID)
                                                  .Select(cln => cln.LastName).FirstOrDefault();

            ViewBag.PetCarerName = petCarerFirstName + " " + petCarerLastName;
            //---------------------------------------------------------
            var petOwnerFirstName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaID)
                                                   .Select(pofn => pofn.FirstName).FirstOrDefault();
            var petOwnerLastName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaID)
                                                  .Select(poln => poln.LastName).FirstOrDefault();

            ViewBag.PetOwnerName = petOwnerFirstName + " " + petOwnerLastName;
            //---------------------------------------------------------
            var formatStartDate = careTransaction.StartDate.ToString("MMMM dd, yyyy");
            var formatEndDate = careTransaction.EndDate.ToString("MMMM dd, yyyy");

            ViewBag.formatStartDate = formatStartDate;
            ViewBag.formatEndDate = formatEndDate;
            //---------------------------------------------------------
            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetID;
            ViewBag.thisPetsCarersASPNetIdentityID = thisPetsCarersASPNetID;
            ViewBag.loggedInUser = loggedInUser;
            // this was here for double-checking stuff
            ViewBag.thisPetsCarersID = thisPetsCarersID;
            //---------------------------------------------------------

            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                           still in Appointment Completion -- PET OWNER -- POST
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/CompleteAppointment/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteAppointment_PetOwner([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport," +
            "Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID," +
            "CareProviderID,PetID,NeededThisVisit,Pending,Confirmed,Completed_PO,Completed_CP")] 
                                                                CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                // the key part here!   [=
                careTransaction.Completed_PO = true;
                
                //-----------------------------------------------------
                db.Entry(careTransaction).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("CompleteConfirmation",
                                        new { ct_id = careTransaction.TransactionID });
            }

            //---------------------------------------------------------
            return View(careTransaction);
        }
        //===============================================================================
        //                                     Appointment COMPLETION -- PET CARER -- GET
        //===============================================================================
        // GET: CareTransactions/CompleteAppointment/5
        public ActionResult CompleteAppointment_PetCarer(int? id)
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
            //---------------------------------------------------------
            // to make sure only the pet's owner & carer can see this page!
            var thisCareTransactionPetID = careTransaction.PetID;

            var thisPetsOwnersID = careTransaction.PetOwnerID;

            var thisPetsOwnersPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisPetsOwnersID)
                                                      .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsOwnersASPNetID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaID)
                                                        .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                        .FirstOrDefault();

            //---------------------------------------------------------
            var thisPetsCarersID = careTransaction.CareProviderID;

            var thisPetsCarersPetopiaID = db.CareProviders.Where(cp => cp.CareProviderID == thisPetsCarersID)
                                                          .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisPetsCarersASPNetID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaID)
                                                        .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                        .FirstOrDefault();
            //---------------------------------------------------------
            var loggedInUser = User.Identity.GetUserId();

            //---------------------------------------------------------------------------
            var petName = db.Pets.Where(p => p.PetID == careTransaction.PetID)
                                 .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = petName;
            //---------------------------------------------------------
            var petCarerFirstName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaID)
                                                   .Select(cfn => cfn.FirstName).FirstOrDefault();
            var petCarerLastName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaID)
                                                  .Select(cln => cln.LastName).FirstOrDefault();

            ViewBag.PetCarerName = petCarerFirstName + " " + petCarerLastName;
            //---------------------------------------------------------
            var petOwnerFirstName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaID)
                                                   .Select(pofn => pofn.FirstName).FirstOrDefault();
            var petOwnerLastName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaID)
                                                  .Select(poln => poln.LastName).FirstOrDefault();

            ViewBag.PetOwnerName = petOwnerFirstName + " " + petOwnerLastName;
            //---------------------------------------------------------
            var formatStartDate = careTransaction.StartDate.ToString("MMMM dd, yyyy");
            var formatEndDate = careTransaction.EndDate.ToString("MMMM dd, yyyy");

            ViewBag.formatStartDate = formatStartDate;
            ViewBag.formatEndDate = formatEndDate;
            //---------------------------------------------------------
            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetID;
            ViewBag.thisPetsCarersASPNetIdentityID = thisPetsCarersASPNetID;
            ViewBag.loggedInUser = loggedInUser;
            //---------------------------------------------------------
            // trying to find out WTF?! (this is CompleteAppointment_PetCarer() GET)
            ViewBag.PO_Complete = careTransaction.Completed_PO;
            // so this ^^^ is showing true ..... I KNOW WHAT IT IS!!!!!
            //---------------------------------------------------------

            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                           still in Appointment Completion -- PET CARER -- POST
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/CompleteAppointment/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteAppointment_PetCarer([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport," +
            "Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID," +
            "CareProviderID,PetID,NeededThisVisit,Pending,Confirmed,Completed_PO,Completed_CP")]
                                                        CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                // the key element here!   [=
                careTransaction.Completed_CP = true;

                db.Entry(careTransaction).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("CompleteConfirmation",
                                        new { ct_id = careTransaction.TransactionID } );
            }

            //---------------------------------------------------------
            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                                still in Appointment Completion -- CONFIRM BOTH
        //-------------------------------------------------------------------------------
        public ActionResult CompleteConfirmation(int? ct_id)
        {
            // pull in the pet care appointment:
            if (ct_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(ct_id);

            if (careTransaction == null)
            {
                return HttpNotFound();
            }
            //---------------------------------------------------------
            // get pet name -- we don't need much else here   [=
            var PetID = careTransaction.PetID;

            var PetName = db.Pets.Where(pID => pID.PetID == PetID)
                                 .Select(pn => pn.PetName).FirstOrDefault();
            ViewBag.PetID = PetID;
            ViewBag.PetName = PetName;
            //---------------------------------------------------------
            // and i guess make sure only Pet Owner/Carer can see this.....
            var loggedInUser = User.Identity.GetUserId();

            var thisPetOwnerID = careTransaction.PetOwnerID;

            var thisPetOwnerPetopiaID = db.PetOwners.Where(poID => poID.PetOwnerID == thisPetOwnerID)
                                          .Select(puID => puID.UserID).FirstOrDefault();

            var thisPetCarerID = careTransaction.CareProviderID;

            var thisPetCarerPetopiaID = db.CareProviders.Where(cpID => cpID.CareProviderID == thisPetCarerID)
                                                        .Select(puID => puID.UserID).FirstOrDefault();
            //---------------------------------------------------------

            return View();
        }
        //===============================================================================
        //                                                    Appointment CANCEL \ DELETE
        //                                             for when PET OWNER cancel\deletes!
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
            //---------------------------------------------------------
            // to make sure only the pet's owner can see this page!
            var thisPetsID = db.CareTransactions.Where(p => p.TransactionID == id)
                                                .Select(pID => pID.PetID).FirstOrDefault();

            var thisPetsOwnersID = db.CareTransactions.Where(ct => ct.TransactionID == id)
                                                      .Select(poID => poID.PetOwnerID).FirstOrDefault();

            var thisPetsOwnersPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisPetsOwnersID)
                                                      .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsOwnersASPNetID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaID)
                                                        .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                        .FirstOrDefault();

            //---------------------------------------------------------
            var loggedInUser = User.Identity.GetUserId();

            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetID;
            ViewBag.loggedInUser = loggedInUser;

            //---------------------------------------------------------
            var thisPetsName = db.Pets.Where(p => p.PetID == thisPetsID)
                                      .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetsName = thisPetsName;

            //---------------------------------------------------------

            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                                           still in Appointment Cancel \ Delete
        //                                             for when PET OWNER cancel\deletes!
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // that ^^^ is the name it comes with (since i'm using 'confirm'-type names)

            CareTransaction careTransaction = db.CareTransactions.Find(id);

            // PULL IN THE OWNER & CARER EMAILS HERE -- 
            // will putting it here work, to send the email before the ID's are deleted?

            // THE EMAIL STUFF GOES IN CONFIRMATIONS 
            //---------------------------------------------------------------------------
            // SEND EMAILS TO (OWNER &) CARER                Cancel_[Delete]_Confirmation
            //
            // try to pull Owner & Carer email addresses:
            var thisPetOwnerID = careTransaction.PetOwnerID;

            var thisPetOwnerPetopiaID = db.PetOwners.Where(poID => poID.PetOwnerID == thisPetOwnerID)
                                                    .Select(puID => puID.UserID).FirstOrDefault();

            var thisPetOwnerAspID = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                   .Select(aspID => aspID.ASPNetIdentityID).FirstOrDefault();

            var thisOwnerEmail = db.ASPNetUsers.Where(aspID => aspID.Id == thisPetOwnerAspID)
                                               .Select(poEm => poEm.Email).FirstOrDefault();
            //---------------------------------------------------------
            var thisPetCarerID = careTransaction.CareProviderID;

            var thisPetCarerPetopiaID = db.PetOwners.Where(poID => poID.PetOwnerID == thisPetCarerID)
                                                    .Select(puID => puID.UserID).FirstOrDefault();

            var thisPetCarerAspID = db.PetopiaUsers.Where(puID => puID.UserID == thisPetCarerPetopiaID)
                                                   .Select(aspID => aspID.ASPNetIdentityID).FirstOrDefault();

            var thisCarerEmail = db.ASPNetUsers.Where(aspID => aspID.Id == thisPetCarerAspID)
                                               .Select(poEm => poEm.Email).FirstOrDefault();
            //---------------------------------------------------------
            try
            {
                var EmailSubject_to_Carer = "[Petopia] Pet Owner has canceled their appointment with you";
                var EmailBody_to_Carer = "Hi! A Petopia User has canceled one of their pet care " +
                    "appointments with you, please navigate over to http://petopia.azurewebsites.net " +
                    "to track all of your appointments.";

                var EmailSubject_to_Owner = "[Petopia] Your Pet Care Appointment Cancel Confirmation";
                var EmailBody_to_Owner = "You requested to cancel your scheduled Pet Care Appointment." +
                    "Your selected Pet Care Provider has been notified.  Thank you for using Petopia," +
                    "please visit when you need Pet Care next time!";

                MailAddress FromEmail = new MailAddress(ConfigurationManager.AppSettings["gmailAccount"]);
                MailAddress ToEmail_Carer = new MailAddress(thisCarerEmail);
                MailAddress ToEmail_Owner = new MailAddress(thisOwnerEmail);

                MailMessage mail_to_carer = new MailMessage(FromEmail, ToEmail_Carer);
                MailMessage mail_to_owner = new MailMessage(FromEmail, ToEmail_Owner);

                mail_to_carer.Subject = EmailSubject_to_Carer;
                mail_to_carer.Body = EmailBody_to_Carer;

                mail_to_owner.Subject = EmailSubject_to_Owner;
                mail_to_owner.Body = EmailBody_to_Owner;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager
                    .AppSettings["gmailAccount"], ConfigurationManager.AppSettings["gmailPassword"]);
                smtp.EnableSsl = true;
                smtp.Send(mail_to_carer);
            }
            catch (Exception e)
            {
            }
            //---------------------------------------------------------

            db.CareTransactions.Remove(careTransaction);

            db.SaveChanges();

            return RedirectToAction("DeleteConfirmation");
        }
        //-------------------------------------------------------------------------------
        //                                           still in Appointment Cancel \ Delete
        //                                             for when PET OWNER cancel\deletes!
        //-------------------------------------------------------------------------------
        // GET: CareTransactions/DeleteConfirmation
        public ActionResult DeleteConfirmation()
        {
            return View();
        }
        //===============================================================================
        //===============================================================================
        //                                        Appointment **DECLINE** \ DELETE -- GET
        //                                   for when PET CARE PROVIDER declines\deletes!
        //===============================================================================
        // GET: CareTransactions/DeclineAppointment/5
        public ActionResult DeclineAppointment(int? ct_id)
        {
            if (ct_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CareTransaction careTransaction = db.CareTransactions.Find(ct_id);

            if (careTransaction == null)
            {
                return HttpNotFound();
            }
            //---------------------------------------------------------
            // to make sure only the pet's requested carer can see this page!
            var thisPetsCarersID = db.CareTransactions.Where(ct => ct.TransactionID == ct_id)
                                                      .Select(poID => poID.CareProviderID).FirstOrDefault();

            var thisPetsCarersPetopiaID = db.CareProviders.Where(po => po.CareProviderID == thisPetsCarersID)
                                                      .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsCarersASPNetID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaID)
                                                        .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                        .FirstOrDefault();

            //---------------------------------------------------------
            var loggedInUser = User.Identity.GetUserId();

            ViewBag.thisPetsCarersASPNetIdentityID = thisPetsCarersASPNetID;
            ViewBag.loggedInUser = loggedInUser;

            //---------------------------------------------------------
            var thisPetsName = db.Pets.Where(p => p.PetID == careTransaction.PetID)
                                      .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetsName = thisPetsName;

            //---------------------------------------------------------
            // just for testing:
            ViewBag.this_CT_ID = careTransaction.TransactionID;
            //---------------------------------------------------------

            return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
        //                                           still in Appointment DECLINE -- POST
        //                                      for when PET CARER declines appt request!
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/DeclineAppointment/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeclineAppointment(int ct_id)
        {
            // hope to God these modifications work like think..... nope -- not yet.....

            CareTransaction careTransaction = db.CareTransactions.Find(ct_id);

            // PULL IN THE OWNER & CARER EMAILS HERE -- 
            // will putting it here work, to send the email before the ID's are deleted?

            // THE EMAIL STUFF GOES IN CONFIRMATIONS 
            //---------------------------------------------------------------------------
            // SEND EMAILS TO (OWNER &) CARER               Decline_[Delete]_Confirmation
            //
            // try to pull Owner & Carer email addresses:
            var thisPetOwnerID = careTransaction.PetOwnerID;

            var thisPetOwnerPetopiaID = db.PetOwners.Where(poID => poID.PetOwnerID == thisPetOwnerID)
                                                    .Select(puID => puID.UserID).FirstOrDefault();

            var thisPetOwnerAspID = db.PetopiaUsers.Where(puID => puID.UserID == thisPetOwnerPetopiaID)
                                                   .Select(aspID => aspID.ASPNetIdentityID).FirstOrDefault();

            var thisOwnerEmail = db.ASPNetUsers.Where(aspID => aspID.Id == thisPetOwnerAspID)
                                               .Select(poEm => poEm.Email).FirstOrDefault();
            //---------------------------------------------------------
            var thisPetCarerID = careTransaction.CareProviderID;

            var thisPetCarerPetopiaID = db.PetOwners.Where(poID => poID.PetOwnerID == thisPetCarerID)
                                                    .Select(puID => puID.UserID).FirstOrDefault();

            var thisPetCarerAspID = db.PetopiaUsers.Where(puID => puID.UserID == thisPetCarerPetopiaID)
                                                   .Select(aspID => aspID.ASPNetIdentityID).FirstOrDefault();

            var thisCarerEmail = db.ASPNetUsers.Where(aspID => aspID.Id == thisPetCarerAspID)
                                               .Select(poEm => poEm.Email).FirstOrDefault();
            //---------------------------------------------------------
            try
            {
                var EmailSubject_to_Owner = "[Petopia] Pet Carer has declined your appointment request";
                var EmailBody_to_Owner = "Hi! The Petopia Care Provider you requested " +
                    "was unable to fulfill your Pet Care Appointment request, and has " +
                    "declined.  Thank you for using Petopia, please visit us again the " +
                    "next time you need pet care!";

                var EmailSubject_to_Carer = "[Petopia] Your Pet Care Appointment Decline Confirmation";
                var EmailBody_to_Carer = "You declined one of your Pet Care Appointment" +
                    "requets.  The Pet Owner has been notified.  Thank you for using" +
                    "Petopia, please check in with us regularly at http://petopia.azurewebsites.net";

                MailAddress FromEmail = new MailAddress(ConfigurationManager.AppSettings["gmailAccount"]);
                MailAddress ToEmail_Carer = new MailAddress(thisCarerEmail);
                MailAddress ToEmail_Owner = new MailAddress(thisOwnerEmail);

                MailMessage mail_to_carer = new MailMessage(FromEmail, ToEmail_Carer);
                MailMessage mail_to_owner = new MailMessage(FromEmail, ToEmail_Owner);

                mail_to_carer.Subject = EmailSubject_to_Carer;
                mail_to_carer.Body = EmailBody_to_Carer;

                mail_to_owner.Subject = EmailSubject_to_Owner;
                mail_to_owner.Body = EmailBody_to_Owner;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager
                    .AppSettings["gmailAccount"], ConfigurationManager.AppSettings["gmailPassword"]);
                smtp.EnableSsl = true;
                smtp.Send(mail_to_carer);
            }
            catch (Exception e)
            {
            }
            //---------------------------------------------------------

            db.CareTransactions.Remove(careTransaction);

            db.SaveChanges();

            return RedirectToAction("DeclineConfirmation");
        }
        //-------------------------------------------------------------------------------
        //                                                   still in Appointment DECLINE
        //                                         for when PET CARER declines a request!
        //-------------------------------------------------------------------------------
        // GET: CareTransactions/DeleteConfirmation
        public ActionResult DeclineConfirmation()
        {
            return View();
        }
        //===============================================================================
        //===============================================================================
        //                                                           APPOINTMENT LISTINGS
        //===============================================================================
        //                                      'MyAppointments' -- Pet Owner & Pet Carer
        //-------------------------------------------------------------------------------
        // GET: CareTransactions
        public ActionResult MyAppointments()
        {
            // the logged-in user
            var identityID = User.Identity.GetUserId();

            // the logged-in user's PetopiaUserID
            var petopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                               .Select(u => u.UserID).FirstOrDefault();

            // getting the FK column 'UserID' in the 'PetOwners' table
            var petOwnerID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
                                         .Select(po => po.PetOwnerID).FirstOrDefault();

            // getting the FK column 'UserID' in the 'CareProviders' table
            var careProviderID = db.CareProviders.Where(u => u.UserID == petopiaUserID)
                                                 .Select(cp => cp.CareProviderID).FirstOrDefault();

            // determining 'IsOwner' or 'IsProvivder' status of this PetopiaUser
            var isPetOwner = db.PetopiaUsers.Where(pu => pu.UserID == petopiaUserID)
                                            .Select(isp => isp.IsOwner).FirstOrDefault();

            var isPetCarer = db.PetopiaUsers.Where(pu => pu.UserID == petopiaUserID)
                                            .Select(ipc => ipc.IsProvider).FirstOrDefault();

            ViewBag.identityID = identityID;
            ViewBag.petopiaUserID = petopiaUserID;
            ViewBag.isPetOwner = isPetOwner;
            ViewBag.isPetCarer = isPetCarer;
            //---------------------------------------------------------
            // this Pet Owner (from the CareTransactions table)
            var thisPetOwner = db.CareTransactions.Where(ct => ct.PetOwnerID == petOwnerID)
                                                  .Select(tpo => tpo.PetOwnerID).FirstOrDefault();

            var thisPetOwnerPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisPetOwner)
                                                    .Select(puID => puID.UserID).FirstOrDefault();

            ViewBag.petOwnerID = petOwnerID;
            ViewBag.thisPetOwner = thisPetOwner;

            //---------------------------------------------------------
            // this Care Provider (from the CareTransactions table)
            var thisCareProvider = db.CareTransactions.Where(ct => ct.CareProviderID == careProviderID)
                                                      .Select(tcp => tcp.CareProviderID).FirstOrDefault();

            var thisPetCarerPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisCareProvider)
                                                    .Select(puID => puID.UserID).FirstOrDefault();

            ViewBag.careProviderID = careProviderID;
            ViewBag.thisCareProvider = thisCareProvider;

            // ONLY for double-checking crap   [=   (including db)
            //var petOwner_UserID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
            //                                  .Select(po => po.UserID).FirstOrDefault();
            //---------------------------------------------------------          

            CareTransactionViewModel Vmodel = new CareTransactionViewModel();
            //---------------------------------------------------------
            // still inside 'MyAppointments()'                                    PENDING
            //---------------------------------------------------------------------------
            Vmodel.ApptInfoListPending = (from ct in db.CareTransactions
                where (ct.PetOwnerID == thisPetOwner || ct.CareProviderID == thisCareProvider)
                        && (ct.Pending)
                        && (ct.EndDate >= DateTime.Today)
                orderby ct.StartDate

                join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                join p in db.Pets on ct.PetID equals p.PetID

                select new CareTransactionViewModel.ApptInfo
                {
                    PetName = p.PetName,
                    PetOwnerName = puO.FirstName + " " + puO.LastName,
                    PetOwnerPetopiaID = puO.UserID, PetCarerPetopiaID = puP.UserID,
                    PetCarerName = puP.FirstName + " " + puP.LastName,

                    StartDate = ct.StartDate, EndDate = ct.EndDate,
                    StartTime = ct.StartTime, EndTime = ct.EndTime,

                    NeededThisVisit = ct.NeededThisVisit,
                    CareProvided = ct.CareProvided,
                    CareReport = ct.CareReport,
                    Charge = ct.Charge, Tip = ct.Tip,

                    PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                    PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                    PetID = ct.PetID, PetOwnerID = ct.PetOwnerID,
                    PetCarerID = ct.CareProviderID,
                    CareTransactionID = ct.TransactionID,

                    Pending = ct.Pending, Confirmed = ct.Confirmed

                }).ToList();

            //---------------------------------------------------------
            // still inside 'MyAppointments()'                                  CONFIRMED
            //---------------------------------------------------------------------------
            Vmodel.ApptInfoListConfirmed = (from ct in db.CareTransactions
                where (ct.PetOwnerID == thisPetOwner || ct.CareProviderID == thisCareProvider)
                        && (ct.Confirmed)
                        && (ct.EndDate >= DateTime.Today)
                orderby ct.StartDate

                join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                join aspO in db.ASPNetUsers on puO.ASPNetIdentityID equals aspO.Id
                join aspP in db.ASPNetUsers on puP.ASPNetIdentityID equals aspP.Id
                join p in db.Pets on ct.PetID equals p.PetID

                // 'MyAppointments()' CONFIRMED TAB
                select new CareTransactionViewModel.ApptInfo
                {
                    PetName = p.PetName,
                    PetOwnerName = puO.FirstName + " " + puO.LastName,
                    PetOwner_Email = aspO.Email, PetOwner_MainPhone = puO.MainPhone,

                    PetOwnerPetopiaID = puO.UserID, PetCarerPetopiaID = puP.UserID,

                    PetCarerName = puP.FirstName + " " + puP.LastName,
                    PetCarer_Email = aspP.Email, PetCarer_MainPhone = puP.MainPhone,

                    StartDate = ct.StartDate, EndDate = ct.EndDate,
                    StartTime = ct.StartTime, EndTime = ct.EndTime,

                    NeededThisVisit = ct.NeededThisVisit,
                    CareProvided = ct.CareProvided,
                    CareReport = ct.CareReport,
                    Charge = ct.Charge, Tip = ct.Tip,

                    PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                    PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                    PetID = ct.PetID, PetOwnerID = ct.PetOwnerID,
                    PetCarerID = ct.CareProviderID,
                    CareTransactionID = ct.TransactionID,

                    Pending = ct.Pending, Confirmed = ct.Confirmed,
                    Completed_PO = ct.Completed_PO, Completed_CP = ct.Completed_CP

                }).ToList();

            //---------------------------------------------------------
            // still inside 'MyAppointments()'                                   FINISHED
            //---------------------------------------------------------------------------
            Vmodel.ApptInfoListFinished = (from ct in db.CareTransactions
                where (ct.PetOwnerID == thisPetOwner || ct.CareProviderID == thisCareProvider)
                        &&  (ct.EndDate < DateTime.Today)
                orderby ct.StartDate

                join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                join p in db.Pets on ct.PetID equals p.PetID

                select new CareTransactionViewModel.ApptInfo
                {
                    PetName = p.PetName,
                    PetOwnerName = puO.FirstName + " " + puO.LastName,
                    PetOwnerPetopiaID = puO.UserID, PetCarerPetopiaID = puP.UserID,
                    PetCarerName = puP.FirstName + " " + puP.LastName,

                    StartDate = ct.StartDate, EndDate = ct.EndDate,
                    StartTime = ct.StartTime, EndTime = ct.EndTime,

                    NeededThisVisit = ct.NeededThisVisit,
                    CareProvided = ct.CareProvided,
                    CareReport = ct.CareReport,
                    Charge = ct.Charge, Tip = ct.Tip,

                    PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                    PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                    PetID = ct.PetID, PetOwnerID = ct.PetOwnerID,
                    PetCarerID = ct.CareProviderID,
                    CareTransactionID = ct.TransactionID,

                    Pending = ct.Pending, Confirmed = ct.Confirmed,
                    Completed_PO = ct.Completed_PO, Completed_CP = ct.Completed_CP

                }).ToList();

            //---------------------------------------------------------
            return View(Vmodel);
        }
        //-------------------------------------------------------------------------------
        //                                             'MyPetsAppointments' -- Pet Owners
        //-------------------------------------------------------------------------------
        // GET: CareTransactions/MyPetsAppointments/5
        public ActionResult MyPetsAppointments(int? pet_id)
        {
            var thisPetID = db.Pets.Where(ct => ct.PetID == pet_id)
                                   .Select(pID => pID.PetID).FirstOrDefault();

            var thisPetsName = db.Pets.Where(pn => pn.PetID == thisPetID)
                                      .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.thisPetID = thisPetID;
            ViewBag.thisPetsName = thisPetsName;
            //---------------------------------------------------------
            // to make sure only the pet's owner can see this page!
            var thisPetsOwnersID = db.Pets.Where(ct => ct.PetID == pet_id)
                                          .Select(poID => poID.PetOwnerID).FirstOrDefault();

            var thisPetsOwnersPetopiaUserID = db.PetOwners.Where(po => po.PetOwnerID == thisPetsOwnersID)
                                                          .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();
            var loggedInUser = User.Identity.GetUserId();

            ViewBag.thisPetsOwnersID = thisPetsOwnersID;
            ViewBag.thisPetsOwnersPetopiaUserID = thisPetsOwnersPetopiaUserID;
            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;

            CareTransactionViewModel Vmodel = new CareTransactionViewModel();
            //---------------------------------------------------------
            // still inside 'MyPetsAppointments()'                                PENDING
            //---------------------------------------------------------------------------
            Vmodel.ApptInfoListPending = (from ct in db.CareTransactions
                where (ct.PetID == thisPetID)
                        && (ct.Pending)
                        && (ct.EndDate >= DateTime.Today)
                orderby ct.StartDate

                join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                join p in db.Pets on ct.PetID equals p.PetID

                select new CareTransactionViewModel.ApptInfo
                {
                    PetName = p.PetName,
                    PetOwnerName = puO.FirstName + " " + puO.LastName,
                    PetOwnerPetopiaID = puO.UserID, PetCarerPetopiaID = puP.UserID,
                    PetCarerName = puP.FirstName + puP.LastName,

                    StartDate = ct.StartDate, EndDate = ct.EndDate,
                    StartTime = ct.StartTime, EndTime = ct.EndTime,

                    NeededThisVisit = ct.NeededThisVisit,
                    CareProvided = ct.CareProvided,
                    CareReport = ct.CareReport,
                    Charge = ct.Charge, Tip = ct.Tip,

                    PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                    PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                    PetID = ct.PetID, PetOwnerID = ct.PetOwnerID,
                    PetCarerID = ct.CareProviderID,
                    CareTransactionID = ct.TransactionID,

                    Pending = ct.Pending, Confirmed = ct.Confirmed

                }).ToList();

            //---------------------------------------------------------
            // still inside 'MyPetsAppointments()'                              CONFIRMED
            //---------------------------------------------------------------------------
            Vmodel.ApptInfoListConfirmed = (from ct in db.CareTransactions
                where (ct.PetID == thisPetID)
                        && (ct.Confirmed)
                        && (ct.EndDate >= DateTime.Today)
                orderby ct.StartDate

                join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                join aspO in db.ASPNetUsers on puO.ASPNetIdentityID equals aspO.Id
                join aspP in db.ASPNetUsers on puP.ASPNetIdentityID equals aspP.Id
                join p in db.Pets on ct.PetID equals p.PetID

                select new CareTransactionViewModel.ApptInfo
                {
                    PetName = p.PetName,
                    PetOwnerName = puO.FirstName + " " + puO.LastName,
                    PetOwner_Email = aspO.Email, PetOwner_MainPhone = puO.MainPhone,

                    PetCarerName = puP.FirstName + puP.LastName, 
                    PetCarer_Email = aspP.Email, PetCarer_MainPhone = puP.MainPhone,

                    PetOwnerPetopiaID = puO.UserID, PetCarerPetopiaID = puP.UserID,

                    StartDate = ct.StartDate, EndDate = ct.EndDate,
                    StartTime = ct.StartTime, EndTime = ct.EndTime,

                    NeededThisVisit = ct.NeededThisVisit,
                    CareProvided = ct.CareProvided,
                    CareReport = ct.CareReport,
                    Charge = ct.Charge, Tip = ct.Tip,

                    PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                    PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                    PetID = ct.PetID, PetOwnerID = ct.PetOwnerID,
                    PetCarerID = ct.CareProviderID,
                    CareTransactionID = ct.TransactionID,

                    Pending = ct.Pending, Confirmed = ct.Confirmed,
                    Completed_PO = ct.Completed_PO, Completed_CP = ct.Completed_CP

                }).ToList();

            //---------------------------------------------------------
            // still inside 'MyPetsAppointments()'                               FINISHED
            //---------------------------------------------------------------------------
            Vmodel.ApptInfoListFinished = (from ct in db.CareTransactions
                where (ct.PetID == thisPetID)
                        && (ct.EndDate < DateTime.Today)
                orderby ct.StartDate

                join cp in db.CareProviders on ct.CareProviderID equals cp.CareProviderID
                join po in db.PetOwners on ct.PetOwnerID equals po.PetOwnerID
                join puO in db.PetopiaUsers on po.UserID equals puO.UserID
                join puP in db.PetopiaUsers on cp.UserID equals puP.UserID
                join p in db.Pets on ct.PetID equals p.PetID

                select new CareTransactionViewModel.ApptInfo
                {
                    PetName = p.PetName,
                    PetOwnerName = puO.FirstName + " " + puO.LastName,
                    PetOwnerPetopiaID = puO.UserID, 

                    PetCarerPetopiaID = puP.UserID, 
                    PetCarerName = puP.FirstName + " " + puP.LastName,

                    StartDate = ct.StartDate, EndDate = ct.EndDate,
                    StartTime = ct.StartTime, EndTime = ct.EndTime,

                    NeededThisVisit = ct.NeededThisVisit,
                    CareProvided = ct.CareProvided,
                    CareReport = ct.CareReport,
                    Charge = ct.Charge, Tip = ct.Tip,

                    PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                    PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                    PetID = ct.PetID, PetOwnerID = ct.PetOwnerID,
                    PetCarerID = ct.CareProviderID,
                    CareTransactionID = ct.TransactionID,

                    Pending = ct.Pending, Confirmed = ct.Confirmed,
                    Completed_PO = ct.Completed_PO, Completed_CP = ct.Completed_CP

                }).ToList();

            //---------------------------------------------------------
            return View(Vmodel);
        }
        //===============================================================================
        //===============================================================================
        //                                                                This Stuff.....
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
        //===============================================================================
        //===============================================================================
        //===============================================================================
        //===============================================================================
        public ActionResult test_crap()
        {
            var identityID = User.Identity.GetUserId();

            // the currently logged-in user
            var petopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                               .Select(u => u.UserID).FirstOrDefault();

            // this currently logged-in user: get the FK 'UserID' from the 'PetOwners' table
            var petOwnerID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
                                         .Select(po => po.PetOwnerID).FirstOrDefault();

            // this currently logged-in user: this is ONLY for double-checking crap   [=
            var petOwner_UserID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
                                              .Select(po => po.UserID).FirstOrDefault();

            // this currently logged-in user: still just checking
            var user_Email = db.ASPNetUsers.Where(u => u.Id == identityID)
                                           .Select(ue => ue.Email).FirstOrDefault();

            // this currently logged-in user -- a Pet Owner -- via CareTransactions
            //    (instead of 'userAppts' like in 'MyAppointments')(and not a list)
            var thisPetOwner = db.CareTransactions.Where(ct => ct.PetOwnerID == petOwnerID)
                                                  .Select(tpo => tpo.PetOwnerID).FirstOrDefault();

            // This logged-in PetopiaUser(PetOwner)'s Zipcode
            var thisPetOwnerZip = db.PetopiaUsers.Where(poz => poz.UserID == petOwner_UserID)
                                             .Select(poz => poz.ResZipcode).FirstOrDefault();

            // this Pet Owner's Pets -- by name (i hope, haha) -- yeah now it works   [=
            var thisOwnersPets = db.Pets.Where(p => p.PetOwnerID == petOwnerID)
                                        .Select(pn => pn.PetName).ToList();

            // for testing/proofing stuff!
            ViewBag.identityID = "this user's IdentityID: " + identityID;
            ViewBag.petopiaUserID = "this user's PetopiaUserID: " + petopiaUserID;
            ViewBag.petOwnerID = "this user's PetOwnerID: " + petOwnerID;
            ViewBag.petOwner_UserID = "this user's PetOwnerID=>PetopiaUserID: " + petOwner_UserID;
            ViewBag.user_Email = "this user's email: " + user_Email;
            ViewBag.thisPetOwner = "this user's CareTransaction=>PetOwnerID: " + thisPetOwner;
            ViewBag.thisPetOwnerZip = "this logged-in user/PetOwner's ZipCode: " + thisPetOwnerZip;
            ViewBag.thisOwnersPets = "this owner's pet list [1]: " + thisOwnersPets[1];
            ViewBag.thisOwnersPetsList = thisOwnersPets;

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

            // get list of Pet Care Provider + IDs --
            //  where CareProvider Zipcode == currentlyLogged-inUser Zipcode -- it works!
            //   --> THIS IS WHAT MAKES THE BLUE CARDS ON THE 'test_crap' VIEW <--
            testLists.PetCarerList = (from pu in db.PetopiaUsers
                                      where pu.ResZipcode == thisPetOwnerZip
                                      join cp in db.CareProviders on pu.UserID equals cp.UserID
                                      select new CareTransactionViewModel.CareProviderInfo
                                      {
                                          CareProviderID = cp.CareProviderID,
                                          CP_Name = pu.FirstName + " " + pu.LastName,
                                          CP_Zipcode = pu.ResZipcode

                                      }).ToList();

            //---------------------------------------------------------------------------
            // now trying to get those "blue card" results into a SelectList dammit..... 
            //   this really should NOT be as difficult as this has been, 
            //      having tried about 73 slight variations of things now.....
            //        (why are there so many ways to do a SelectList anyway???)
            List<SelectListItem> PetCarerSelectList = (from pu in db.PetopiaUsers
                                                        where pu.ResZipcode == thisPetOwnerZip
                                                        join cp in db.CareProviders on pu.UserID equals cp.UserID
                                                        select new SelectListItem
                                                        {
                                                            Value = cp.CareProviderID.ToString(),
                                                            Text = pu.FirstName + " " + pu.LastName

                                                        }).ToList();

            ViewBag.PetCarerSelectList = PetCarerSelectList;
            // ^^^ this finally works -- at least in appearance..... 
            //     don't know yet, if it will pass the ID correctly  <-- IT DOES NOT!!!!!
            //     why did i have to '.ToString()' it, and will that pass correctly?
            //      since ID's are ints?  how did this work in those other projects?
            //       they were a bit more "direct" and not based off a query like this
            //---------------------------------------------------------------------------
            //
            // now to try the same thing to make a SelectList of the currently logged-in
            //   PetOwner's Pets.....
            List<SelectListItem> ThisOwnersPetsSelectList = (from pn in db.Pets
                                                            where pn.PetOwnerID == thisPetOwner
                                                            select new SelectListItem
                                                            {
                                                                Value = pn.PetID.ToString(),
                                                                Text = pn.PetName

                                                            }).ToList();

            ViewBag.ThisOwnersPetsSelectList = ThisOwnersPetsSelectList;
            // same as with the Pet Carer Select List  <-- DOES NOT PASS ID CORRECTLY
            //---------------------------------------------------------------------------
            //
            // (early) testing & fiddling around w/doing a SelectList of PetCareProviders
            //  + IDs -- with zipcodes that match logged-in user
            var woof = (from pu in db.PetopiaUsers
                        where pu.ResZipcode == thisPetOwnerZip
                        join cp in db.CareProviders on pu.UserID equals cp.UserID
                        select new SelectListItem
                        {
                            Value = "cp.CareProviderID",
                            Text = "pu.FirstName" + "pu.LastName"

                        }).ToList();

            ViewBag.woof = woof;

            // trying various ways to get a stupid-ass SelectList.....
            //
            // SelectList of Pet Carers with matching Zipcode to logged-in user
            ViewBag.CP_byZip_SelectList = new SelectList(db.PetopiaUsers.OrderBy(ln => ln.LastName)
                      .Where(z => z.ResZipcode == thisPetOwnerZip & z.IsProvider), "UserID", "LastName");

            ViewBag.CP_matchZip_SelectList = new SelectList(woof, "CareProviderID", "CP_Name");

            //---------------------------------------------------------
            return View(testLists);
        }                                                
        //===============================================================================
        [HttpPost]                                     // not actually doing anything
        [AllowAnonymous]                                // with this yet   [=
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
            //---------------------------------------------------------
            return View();
        }
        //===============================================================================
    }
}
