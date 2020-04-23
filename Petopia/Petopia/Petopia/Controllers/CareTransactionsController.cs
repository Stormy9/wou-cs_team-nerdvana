using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
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
            return View(db.CareTransactions.ToList());
        }

        //===============================================================================
        // GET: CareTransactions/Details/5
        public ActionResult Details(int? id)
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
            // a possibly useful example of ... something
            //
            //ViewBag.PetOwnerID = new SelectList(db.PetOwner.OrderBy(a => a.AthleteName), "AthleteID", "AthleteName");

            ViewBag.RatingList = ratingSelectList;
            return View();
        }
        //-------------------------------------------------------------------------------
        // POST: CareTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport,Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID,CareProviderID,PetID,NeededThisVisit")] CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                db.CareTransactions.Add(careTransaction);
                db.SaveChanges();

                return RedirectToAction("Index");
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
        public ActionResult EditAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport,Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID,CareProviderID,PetID,NeededThisVisit")] CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(careTransaction).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
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

            return RedirectToAction("Index");
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
        //===============================================================================
        //---------- RATING DROP-DOWN LIST IN 'CREATE (and edit) APPOINTMENT' -----------
        //
        private IList<SelectListItem> ratingSelectList = new List<SelectListItem>
        {
            new SelectListItem
                { Value = "1", Text = "1" },
            new SelectListItem
                { Value = "2", Text = "2" },
            new SelectListItem
                { Value = "3", Text = "3" },
            new SelectListItem
                { Value = "4", Text = "4" },
            new SelectListItem
                { Value = "5", Text = "5" }
        };
        //===============================================================================
        //===============================================================================
        // GET: CareTransactions/Edit/5
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
        // POST: CareTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details: https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteAppointment([Bind(Include = "TransactionID,StartDate,EndDate,StartTime,EndTime,CareProvided,CareReport,Charge,Tip,PC_Rating,PC_Comments,PO_Rating,PO_Comments,PetOwnerID,CareProviderID,PetID,NeededThisVisit")] CareTransaction careTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(careTransaction).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(careTransaction);
        }

        //===============================================================================
    }
}
