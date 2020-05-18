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
        public ActionResult Appts_AdminIndex()
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
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = thisPetName;

            //---------------------------------------------------------------------------
            // getting the Pet Carer name for display!              it worked!   =]
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.LastName).FirstOrDefault();

            ViewBag.PetCarerName = thisCarerFirstName + " " + thisCarerLastName;

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

            ViewBag.PetOwnerName = thisPetOwnerFirstName + " " + thisPetOwnerLastName;

            //---------------------------------------------------------------------------
            // getting start & end dates -- to format the display           it worked!
            var thisStartDate = careTransaction.StartDate.ToString("MM/dd/yyyy");
            var thisEndDate = careTransaction.EndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = thisStartDate;
            ViewBag.ApptEndDate = thisEndDate;

            //---------------------------------------------------------------------------

            return View(careTransaction);
        }
        //===============================================================================
        // GET: CareTransactions/BookAppointment/5
        public ActionResult BookAppointment(int? id)
        {
            //---------------------------------------------------------------------------
            // getting the pet that was passed in:
            var thisPetID = id;
            ViewBag.thisPetId = thisPetID;

            var thisPetName = db.Pets.Where(p => p.PetID == thisPetID)
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.thisPetName = thisPetName;

            //---------------------------------------------------------------------------
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

            //---------------------------------------------------------------------------
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

            //---------------------------------------------------------------------------

            return View(MatchingPetCarers);
        }
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/BookAppointment
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport," +
          "Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID," +
          "CareProviderID,PetID,NeededThisVisit,Pending,Confirmed,Complete_PO,Complete_CP")]
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
                //-----------------------------------------------------------------------

                // this seems to be really important, haha
                careTransaction.PetOwnerID = thisPetOwnerID;

                // and let's add this:
                careTransaction.Pending = true;
                // do we need to set these? will they be null if we don't?
                // cuz it does NOT like these being null (even though they're nullable)
                careTransaction.Confirmed = false;
                careTransaction.Completed_PO = false;       // good for testing for now
                careTransaction.Completed_CP = false;

                //-----------------------------------------------------------------------
                // to make sure only the pet's owner can see this page!

                var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers
                        .Where(pu => pu.UserID == thisPetopiaUserID)
                        .Select(aspnetID => aspnetID.ASPNetIdentityID).FirstOrDefault();

                var loggedInUser = User.Identity.GetUserId();

                ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
                ViewBag.loggedInUser = loggedInUser;
                //-----------------------------------------------------------------------

                db.CareTransactions.Add(careTransaction);
                db.SaveChanges();

                return RedirectToAction("BookConfirmation", 
                                         new { id = careTransaction.TransactionID });
            }

            return View(careTransaction);
        }
        //===============================================================================
        //-------------------------------------------------------------------------------
        // GET: CareTransactions/ConfirmAppointment/5                   base off 'Edit()'
        public ActionResult ConfirmAppointment(int? ct_id)
        {
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
            //---------------------------------------------------------------------------
            // the (careTransaction) is super-important, haha
            return View(careTransaction);
        }
        //===============================================================================
        // POST: CareTransactions/ConfirmAppointment/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport," +
          "Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID," +
          "CareProviderID,PetID,NeededThisVisit")] CareTransaction careTransaction)
        {
            // so why *do* some of these have all that ^^^ and others don't?

            if (ModelState.IsValid)
            {
                db.Entry(careTransaction).State = EntityState.Modified;

                careTransaction.Pending = false;
                careTransaction.Confirmed = true;

                db.SaveChanges();

                return RedirectToAction("EditConfirmation",
                                        new { id = careTransaction.TransactionID });
            }

                return View(careTransaction);
        }
        //-------------------------------------------------------------------------------
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
            //---------------------------------------------------------------------------
            // to make sure only the pet's owner can see this page!
            var thisPetsID = careTransaction.PetID;

            var thisPetsOwnersID = careTransaction.PetOwnerID;

            var thisPetsOwnersPetopiaUserID = db.PetOwners.Where(po => po.PetOwnerID == thisPetsOwnersID)
                                                          .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();
            var loggedInUser = User.Identity.GetUserId();

            var petName = db.Pets.Where(p => p.PetID == thisPetsID)
                                 .Select(pn => pn.PetName).FirstOrDefault();

            var thisPetOwnerZip = db.PetopiaUsers.Where(poz => poz.UserID == thisPetsOwnersPetopiaUserID)
                                                     .Select(poz => poz.ResZipcode).FirstOrDefault();

            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;
            ViewBag.PetName = petName;

            //---------------------------------------------------------------------------
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

            //---------------------------------------------------------------------------
            // to make sure only the pet's owner can see this page!
            var thisPetsID = db.CareTransactions.Where(p => p.TransactionID == id)
                                                .Select(pID => pID.PetID).FirstOrDefault();

            var thisPetsOwnersID = db.CareTransactions.Where(ct => ct.TransactionID == id)
                                                      .Select(poID => poID.PetOwnerID).FirstOrDefault();

            var thisPetsOwnersPetopiaUserID = db.PetOwners.Where(po => po.PetOwnerID == thisPetsOwnersID)
                                                          .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();

            //---------------------------------------------------------
            var thisPetsCarersID = db.CareTransactions.Where(ct => ct.TransactionID == id)
                                                      .Select(cpID => cpID.CareProviderID).FirstOrDefault();

            var thisPetsCarersPetopiaUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisPetsCarersID)
                                                              .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisPetsCarersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();

            //---------------------------------------------------------
            var loggedInUser = User.Identity.GetUserId();

            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.thisPetsCarersASPNetIdentityID = thisPetsCarersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;
            ViewBag.thisPetsCarersID = thisPetsCarersID;

            //---------------------------------------------------------
            var thisPetsName = db.Pets.Where(p => p.PetID == thisPetsID)
                                      .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetsName = thisPetsName;

            //---------------------------------------------------------

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
        // our added 'ActionResult' methods.....
        //===============================================================================
        // GET: CareTransactions/AppointmentConfirmation/5
        public ActionResult BookConfirmation(int? id)
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
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = thisPetName;
            //---------------------------------------------------------------------------
            // getting the Pet Owner name for display!                  it worked!   =]
            var thisOwnerID = careTransaction.PetOwnerID;

            var thisOwnerUserID = db.PetOwners.Where(cp => cp.PetOwnerID == thisOwnerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisOwnerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisOwnerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                   .Select(cpn => cpn.LastName).FirstOrDefault();

            ViewBag.PetOwnerName = thisOwnerFirstName + " " + thisOwnerLastName;
            //---------------------------------------------------------------------------
            // getting the Pet Carer name for display!                   it worked!   =]
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
            //---------------------------------------------------------------------------
            // getting start & end dates -- to format the display           it worked!
            var thisStartDate = careTransaction.StartDate.ToString("MM/dd/yyyy");
            var thisEndDate = careTransaction.EndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = thisStartDate;
            ViewBag.ApptEndDate = thisEndDate;

            //---------------------------------------------------------------------------
            // SEND EMAILS TO OWNER & CARER                       AppointmentConfirmation
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
                                     .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = thisPetName;

            //---------------------------------------------------------------------------
            // getting the Pet Owner name for display!                  it worked!   =]
            var thisOwnerID = careTransaction.PetOwnerID;

            var thisOwnerUserID = db.PetOwners.Where(cp => cp.PetOwnerID == thisOwnerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisOwnerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisOwnerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisOwnerUserID)
                                                   .Select(cpn => cpn.LastName).FirstOrDefault();

            ViewBag.PetOwnerName = thisOwnerFirstName + " " + thisOwnerLastName;

            //---------------------------------------------------------------------------
            // getting the Pet Carer name for display!                      it worked!
            var thisCarerID = careTransaction.CareProviderID;

            var thisCarerUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisCarerID)
                                                  .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisCarerFirstName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.FirstName).FirstOrDefault();

            var thisCarerLastName = db.PetopiaUsers.Where(cp => cp.UserID == thisCarerUserID)
                                                    .Select(cpn => cpn.LastName).FirstOrDefault();

            ViewBag.PetCarerName = thisCarerFirstName + " " + thisCarerLastName;

            ViewBag.PetCarers = new SelectList(db.PetopiaUsers.OrderBy(c => c.LastName),
                                                "CarerID", "CarerName", careTransaction.CareProviderID);

            //---------------------------------------------------------------------------
            // getting start & end dates -- to format the display            it worked!
            var thisStartDate = careTransaction.StartDate.ToString("MM/dd/yyyy");
            var thisEndDate = careTransaction.EndDate.ToString("MM/dd/yyyy");

            ViewBag.ApptStartDate = thisStartDate;
            ViewBag.ApptEndDate = thisEndDate;
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
            //---------------------------------------------------------------------------
            // to make sure only the pet's owner & carer can see this page!
            var thisCareTransactionPetID = db.CareTransactions.Where(ct => ct.TransactionID == id)
                                                              .Select(ctPID => ctPID.PetID).FirstOrDefault();

            var thisPetsOwnersID = db.CareTransactions.Where(ct => ct.TransactionID == id)
                                                      .Select(poID => poID.PetOwnerID).FirstOrDefault();

            var thisPetsOwnersPetopiaUserID = db.PetOwners.Where(po => po.PetOwnerID == thisPetsOwnersID)
                                                          .Select(pUID => pUID.UserID).FirstOrDefault();

            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();

            //---------------------------------------------------------
            var thisPetsCarersID = db.CareTransactions.Where(ct => ct.TransactionID == id)
                                                      .Select(cpID => cpID.CareProviderID).FirstOrDefault();

            var thisPetsCarersPetopiaUserID = db.CareProviders.Where(cp => cp.CareProviderID == thisPetsCarersID)
                                                              .Select(cpID => cpID.UserID).FirstOrDefault();

            var thisPetsCarersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();
            //---------------------------------------------------------
            var loggedInUser = User.Identity.GetUserId();

            //---------------------------------------------------------
            var petName = db.Pets.Where(p => p.PetID == careTransaction.PetID)
                                 .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.PetName = petName;

            //---------------------------------------------------------
            var petCarerFirstName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaUserID)
                                                   .Select(cfn => cfn.FirstName).FirstOrDefault();
            var petCarerLastName = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsCarersPetopiaUserID)
                                                  .Select(cln => cln.LastName).FirstOrDefault();

            ViewBag.PetCarerName = petCarerFirstName + " " + petCarerLastName;

            //---------------------------------------------------------

            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.thisPetsCarersASPNetIdentityID = thisPetsCarersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;
            ViewBag.thisPetsCarersID = thisPetsCarersID;

            //---------------------------------------------------------------------------

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
        //===============================================================================
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

            // determining 'IsOwner' or 'IsProvivder' status of this PetopiaUser
            var isPetOwner = db.PetopiaUsers.Where(pu => pu.UserID == petopiaUserID)
                                            .Select(isp => isp.IsOwner).FirstOrDefault();

            // getting the FK column 'UserID' in the 'CareProviders' table
            var careProviderID = db.CareProviders.Where(u => u.UserID == petopiaUserID)
                                                 .Select(cp => cp.CareProviderID).FirstOrDefault();

            var isPetCarer = db.PetopiaUsers.Where(pu => pu.UserID == petopiaUserID)
                                            .Select(ipc => ipc.IsProvider).FirstOrDefault();

            // still just checking\proofing
            var user_Email = db.ASPNetUsers.Where(u => u.Id == identityID)
                                           .Select(ue => ue.Email).FirstOrDefault();

            //---------------------------------------------------------
            // proofing stuff
            // this Pet Owner (from the CareTransactions table)
            var thisPetOwner = db.CareTransactions.Where(ct => ct.PetOwnerID == petOwnerID)
                                                  .Select(tpo => tpo.PetOwnerID).FirstOrDefault();

            // this Care Provider (from the CareTransactions table) (proofing)
            var thisCareProvider = db.CareTransactions.Where(ct => ct.CareProviderID == careProviderID)
                                                      .Select(tcp => tcp.CareProviderID).FirstOrDefault();

            // ONLY for double-checking crap   [=   (including db)
            //var petOwner_UserID = db.PetOwners.Where(u => u.UserID == petopiaUserID)
            //                                  .Select(po => po.UserID).FirstOrDefault();
            //---------------------------------------------------------

            // for checking stuff in the view + proofing
            ViewBag.isPetOwner = isPetOwner;
            ViewBag.isPetCarer = isPetCarer;
            // mostly (or only) for proofing stuff   [=
            ViewBag.identityID = identityID;
            ViewBag.petopiaUserID = petopiaUserID;
            ViewBag.petOwnerID = petOwnerID;
            ViewBag.petCarerID = careProviderID;
            ViewBag.user_Email = user_Email;
            ViewBag.thisPetOwner = thisPetOwner;
            ViewBag.thisCareProvider = thisCareProvider;

            //---------------------------------------------------------
            // still inside 'MyAppointments()'
            //---------------------------------------------------------------------------
            CareTransactionViewModel Vmodel = new CareTransactionViewModel();

            Vmodel.ApptInfoListUpcoming = (from ct in db.CareTransactions
                where ct.PetOwnerID == thisPetOwner ||
                        ct.CareProviderID == thisCareProvider &
                        ct.EndDate >= DateTime.Now
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
                    PetCarerName = puP.FirstName + " " + puP.LastName,

                    StartDate = ct.StartDate, EndDate = ct.EndDate,
                    StartTime = ct.StartTime, EndTime = ct.EndTime,

                    NeededThisVisit = ct.NeededThisVisit,
                    CareProvided = ct.CareProvided, CareReport = ct.CareReport,
                    Charge = ct.Charge, Tip = ct.Tip,

                    PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                    PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                    PetID = ct.PetID, PetOwnerID = ct.PetOwnerID, 
                    PetCarerID = ct.CareProviderID,
                    CareTransactionID = ct.TransactionID
                }).ToList();

            //---------------------------------------------------------
            Vmodel.ApptInfoListPast = (from ct in db.CareTransactions
                    where ct.PetOwnerID == thisPetOwner || 
                          ct.CareProviderID == thisCareProvider & 
                          ct.EndDate < DateTime.Now
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
                        PetCarerName = puP.FirstName + " " + puP.LastName,

                        StartDate = ct.StartDate, EndDate = ct.EndDate,
                        StartTime = ct.StartTime, EndTime = ct.EndTime,

                        NeededThisVisit = ct.NeededThisVisit,
                        CareProvided = ct.CareProvided, CareReport = ct.CareReport,
                        Charge = ct.Charge, Tip = ct.Tip,

                        PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                        PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                        PetID = ct.PetID, PetOwnerID = ct.PetOwnerID, PetCarerID = ct.CareProviderID,
                        CareTransactionID = ct.TransactionID
                    }).ToList();


            return View(Vmodel);
        }
        //===============================================================================
        // GET: CareTransactions/MyPetsAppointments/5
        public ActionResult MyPetsAppointments(int? pet_id)
        {
            var thisPetID = db.Pets.Where(ct => ct.PetID == pet_id)
                                   .Select(pID => pID.PetID).FirstOrDefault();

            var thisPetsName = db.Pets.Where(pn => pn.PetID == thisPetID)
                                      .Select(pn => pn.PetName).FirstOrDefault();

            ViewBag.thisPetID = thisPetID;
            ViewBag.thisPetsName = thisPetsName;
            //---------------------------------------------------------------------------
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

            //---------------------------------------------------------
            // still inside 'MyPetsAppointments()'
            //---------------------------------------------------------------------------
            CareTransactionViewModel Vmodel = new CareTransactionViewModel();

            Vmodel.ApptInfoListUpcoming = (from ct in db.CareTransactions 
                where ct.PetID == thisPetID & 
                      ct.EndDate >= DateTime.Now
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
                    PetCarerName = puP.FirstName + puP.LastName,

                    StartDate = ct.StartDate, EndDate = ct.EndDate,
                    StartTime = ct.StartTime, EndTime = ct.EndTime,

                    NeededThisVisit = ct.NeededThisVisit,
                    CareProvided = ct.CareProvided, CareReport = ct.CareReport,
                    Charge = ct.Charge, Tip = ct.Tip,

                    PC_Rating = ct.PC_Rating, PC_Comments = ct.PC_Comments,
                    PO_Rating = ct.PO_Rating, PO_Comments = ct.PO_Comments,

                    PetID = ct.PetID, PetOwnerID = ct.PetOwnerID, 
                    PetCarerID = ct.CareProviderID,
                    CareTransactionID = ct.TransactionID
                }).ToList();

            //---------------------------------------------------------
            Vmodel.ApptInfoListPast = (from ct in db.CareTransactions
                where ct.PetID == thisPetID & 
                      ct.EndDate < DateTime.Now
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
                    CareTransactionID = ct.TransactionID
                }).ToList();

            return View(Vmodel);
        }
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
            //---------------------------------------------------------------------------
            // GET CURRENTLY LOGGED-IN USER STUFF -- for privacy issues
            //
            // the currently logged-in user:
            var identityID = User.Identity.GetUserId();

            // the currently logged-in user's PetopiaUserID:
            var thisPetopiaUserID = db.PetopiaUsers.Where(u => u.ASPNetIdentityID == identityID)
                                                   .Select(u => u.UserID).FirstOrDefault();

            ViewBag.LoggedInPetopiaUserID = thisPetopiaUserID;
            //---------------------------------------------------------------------------
            // GET THIS TRANSACTION ID -- for proofing
            //
            var thisJobID = thisJob.TransactionID;

            ViewBag.ThisJobID = thisJobID;
            //---------------------------------------------------------------------------
            // GET THS CARE PROVIDER ON *THIS* CareTransaction:
            //
            // get CP_ID outta the CareTransactionID passed in
            var thisPetCarerID = thisJob.CareProviderID;

            // get THIS CP's PetopiaUserID
            var thisPetCarerPetopiaID = db.CareProviders.Where(cp => cp.CareProviderID == thisPetCarerID)
                                                        .Select(pu => pu.UserID).FirstOrDefault();

            ViewBag.ThisPetCarerID = thisPetCarerID;
            ViewBag.ThisPetCarerPetopiaID = thisPetCarerPetopiaID;
            //---------------------------------------------------------------------------
            // PET OWNER ON *THIS* CareTransaction:
            //
            // get the PO_ID outta the CareTransactionID passed in
            var thisPetOwnerID = thisJob.PetOwnerID;

            // get THIS PO's PetopiaUserID
            var thisPetOwnerPetopiaID = db.PetOwners.Where(po => po.PetOwnerID == thisPetOwnerID)
                                                    .Select(pu => pu.UserID).FirstOrDefault();

            ViewBag.ThisPetOwnerID = thisPetOwnerID;
            ViewBag.ThisPetOwnerPetopiaID = thisPetOwnerPetopiaID;
            //---------------------------------------------------------------------------
            // get the PetID outta the CareTransactionID passed in
            var thisPetID = thisJob.PetID;

            ViewBag.ThisPetID = thisPetID;
            //---------------------------------------------------------------------------
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
            //---------------------------------------------------------------------------
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


            var thisPetOwnerHomeAccess = db.PetOwners.Where(puID => puID.PetOwnerID == thisPetOwnerID)
                                                     .Select(poHA => poHA.HomeAccess).FirstOrDefault();

            ViewBag.ThisPetOwnerHomeAccess = thisPetOwnerHomeAccess;

            //---------------------------------------------------------------------------
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

            //---------------------------------------------------------------------------

            return View(thisJob);
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

            return View();
        }
        //===============================================================================
    }
}
