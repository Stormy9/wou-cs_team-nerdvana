using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Petopia.DAL;
using Petopia.Models;
using Petopia.Models.ViewModels;
using SimpleZipCode;

namespace Petopia.Controllers
{
    public class HomeController : Controller
    {
        private PetopiaContext pdb = new PetopiaContext();

        public ActionResult Index()
        {
            //var identityID = User.Identity.GetUserId();
            //DAL.PetopiaUser currentUser = pdb.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID).First();

            var ZipCodes = ZipCodeSource.FromMemory().GetRepository();

            var OwnerLocation = ZipCodes.Get("97301");
            var ZipCodesNearOwner = ZipCodes.RadiusSearch(OwnerLocation, 10);
            
            List<String> zipsList = new List<String>();

            foreach(ZipCode zip in ZipCodesNearOwner)
            {
                zipsList.Add(zip.PostalCode);
            }

            ViewBag.ZipList = String.Join(",", zipsList.ToArray());

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
            //    compared to results in MSSQL Server Manager
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
                                                  CP_PU_ID = pu.UserID,
                                                  CP_Name = pu.FirstName + " " + pu.LastName,
                                                  PU_Zipcode = pu.ResZipcode,

                                                  CP_Profile_Pic = pu.ProfilePhoto,
                                                  ExperienceDetails = cp.ExperienceDetails,
                                                  ProviderAverageRating = cp.AverageRating,

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

                                              });

            var ZipCodes = ZipCodeSource.FromMemory().GetRepository();

            var OwnerLocation = ZipCodes.Get(searchZip);
            var ZipCodesNearOwner = ZipCodes.RadiusSearch(OwnerLocation, 10);

            List<String> zipsList = new List<String>();

            foreach (ZipCode zip in ZipCodesNearOwner)
            {
                zipsList.Add(zip.PostalCode);
            }

            ViewBag.ZipList = String.Join(",", zipsList.ToArray());


            return View(carerSearch);
        }
        //===============================================================================
        public ActionResult PetOwnerSearchResult(string searchZip)   // string searchZip
        {
            // this was here just to prove the Query worked   [=
            //    compared to results in MSSQL Server Manager
            //searchZip = "97301";

            // in case we want it.....
            ViewBag.SearchZip = searchZip;

            SearchViewModel ownerSearch = new SearchViewModel();

            ownerSearch.PetOwnerSearchList = (from pu in pdb.PetopiaUsers
                                              where pu.ResZipcode.Contains(searchZip) && pu.IsOwner

                                              join po in pdb.PetOwners on pu.UserID equals po.UserID
                                              join ub in pdb.UserBadges on po.UserID equals ub.UserID

                                              select new SearchViewModel.PetOwnerSearch
                                              {
                                                  PO_ID = po.PetOwnerID,
                                                  PO_PU_ID = pu.UserID,
                                                  PO_Name = pu.FirstName + " " + pu.LastName,
                                                  PU_Zipcode = pu.ResZipcode,

                                                  PO_Profile_Pic = pu.ProfilePhoto,
                                                  GeneralNeeds = po.GeneralNeeds,
                                                  OwnerAverageRating = po.AverageRating,

                                                  IsDogOwner = ub.DogOwner,
                                                  IsCatOwner = ub.CatOwner,
                                                  IsBirdOwner = ub.BirdOwner,
                                                  IsFishOwner = ub.FishOwner,
                                                  IsHorseOwner = ub.HorseOwner,
                                                  IsLivestockOwner = ub.LivestockOwner,
                                                  IsRabbitOwner = ub.RabbitOwner,
                                                  IsReptileOwner = ub.ReptileOwner,
                                                  IsRodentOwner = ub.RodentOwner,
                                                  IsOtherOwner = ub.OtherOwner

                                              });

            return View(ownerSearch);
        }
        //===============================================================================
    }
}