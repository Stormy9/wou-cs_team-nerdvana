using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Petopia.DAL;
using Petopia.Models;
using Petopia.Models.ViewModels;

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

           
                return View();
        }
        //===============================================================================
        public ActionResult About()
        {
            ViewBag.Title = "What does Petopia do?"; 
            ViewBag.Message_PO = "find out how Petopia can make your and your pet's life better";
            ViewBag.Message_PCP = "or have a side hustle doing something you love";

            return View();
        }
        //===============================================================================
        public ActionResult Contact()
        {
            ViewBag.Title = "Contact Petopia";
            ViewBag.Message_PS = "Have a question about services, or need help resolving" +
                                                    " a service issue?";
            ViewBag.Message_PR = "Did you find inappropriate content you would like to report?";
            ViewBag.Message_PT = "Need to report or get help with a technical issue on our site?";

            return View();
        }
        //===============================================================================
        //===============================================================================
        public ActionResult PetCarerSearchResult(string searchZip)   // string searchZip
        {
            // this was here just to prove the Query worked   [=
            //searchZip = "97301";

            // in case we want it.....
            ViewBag.SearchZip = searchZip;

            SearchViewModel carerSearch = new SearchViewModel();

            carerSearch.PetCarerSearchList = (from pu in pdb.PetopiaUsers
                                              where pu.ResZipcode.Contains(searchZip) && pu.IsProvider

                                              join cp in pdb.CareProviders on pu.UserID equals cp.UserID
                                              join ub in pdb.UserBadges on cp.UserID equals ub.UserID

                                              select new SearchViewModel.CareProviderSearch
                                              {
                                                  CP_ID = cp.CareProviderID,
                                                  CP_Name = pu.FirstName + " " + pu.LastName,
                                                  CP_Zipcode = pu.ResZipcode,

                                                  IsDogProvider = ub.DogProvider,
                                                  IsCatProvider = ub.CatProvider,
                                                  IsBirdProvider = ub.BirdProvider,
                                                  IsFishProvider = ub.FishProvider,
                                                  IsHorseProvider = ub.HorseProvider,
                                                  IsLivestockProvider = ub.LivestockProvider,
                                                  IsRabbitProvider = ub.RabbitProvider,
                                                  IsReptileProvider = ub.ReptileProvider,
                                                  IsRodentProvider = ub.RodentProvider,
                                                  IsOtherProvider = ub.OtherProvider

                                              }).ToList();


            return View(carerSearch);
        }
        //===============================================================================
    }
}