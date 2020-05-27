using Microsoft.AspNet.Identity;
using Petopia.DAL;
using Petopia.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminLteMvc.Controllers
{
    /// <summary>
    /// This is an example controller using the AdminLTE NuGet package's CSHTML templates, CSS, and JavaScript
    /// You can delete these, or use them as handy references when building your own applications
    /// </summary>
    public class AdminLteController : Controller
    {
        private PetopiaContext db = new PetopiaContext();
        /// <summary>
        /// The home page of the AdminLTE demo dashboard, recreated in this new system
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var identityID = User.Identity.GetUserId();

            var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                          .Select(x => x.UserID).First();

            AdminPanelViewModel adminPetopiaUser = new AdminPanelViewModel();

            adminPetopiaUser.UserList = db.PetopiaUsers.Select(n => new AdminPanelViewModel.AdminPetopiaUser
            {
                UserID = n.UserID,
                FirstName = n.FirstName,
                LastName = n.LastName,
                ASPNetIdentityID = n.ASPNetIdentityID,
                IsOwner = n.IsOwner,
                IsProvider = n.IsProvider,
                ProfilePhoto = n.ProfilePhoto,
                UserCaption = n.UserCaption,
                GeneralLocation = n.GeneralLocation,
                UserBio = n.UserBio,
                Tagline = n.Tagline,
                MainPhone = n.MainPhone,
                AltPhone = n.AltPhone,
                ResAddress01 = n.ResAddress01,
                ResAddress02 = n.ResAddress02,
                ResCity = n.ResCity,
                ResState = n.ResState,
                ResZipcode = n.ResZipcode
            }).ToList();


            return View(adminPetopiaUser);
        }
        //===============================================================================
        /// <summary>
        /// The color page of the AdminLTE demo, 
        /// demonstrating the AdminLte.Color enum and supporting methods
        /// </summary>
        /// <returns></returns>
        public ActionResult Colors()
        {
            return View();
        }
        //===============================================================================
        //===============================================================================
        public ActionResult Our_Dashboard()
        {
            return View();
        }
        //===============================================================================
        //===============================================================================
    }
}