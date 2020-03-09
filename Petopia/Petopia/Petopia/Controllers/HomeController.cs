using Microsoft.AspNet.Identity;
using Petopia.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Petopia.Controllers
{
    public class HomeController : Controller
    {
        private PetopiaContext pdb = new PetopiaContext();
        public ActionResult Index()
        {
            //var identityID = User.Identity.GetUserId();
            //DAL.PetopiaUser currentUser = pdb.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).First();
            bool loggedIn = User.Identity.IsAuthenticated;
            ViewBag.loggedIn = loggedIn;
            if (loggedIn)
            {
                var identityID = User.Identity.GetUserId();
                DAL.PetopiaUser currentUser = pdb.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).First();
                return View(currentUser);
            }
            else
            {
                return View();
            }
        }
        //-------------------------------------------------------------------------------

        public ActionResult About()
        {
            ViewBag.Title = "What does Petopia do?"; 
            ViewBag.Message_PO = "find out how Petopia can make your and your pet's life better";
            ViewBag.Message_PCP = "or have a side hustle doing something you love";

            return View();
        }
        //-------------------------------------------------------------------------------

        public ActionResult Contact()
        {
            ViewBag.Title = "Contact Petopia";
            ViewBag.Message_PS = "Have a question about services, or need to report a service issue?";
            ViewBag.Message_PT = "Need to report or get help with a technical issue on our site?";

            return View();
        }

    }
}