using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.DAL;

namespace Petopia.Controllers
{
    public class ImageController : Controller
    {
        private PetopiaContext db = new PetopiaContext();
        // GET: Image
        public ActionResult Show(int id)
        {
            var imageData = db.PetopiaUsers.Where(x => x.UserID == id).Select(x => x.ProfilePhoto).First();
            return File(imageData, "image/jpeg");
        }
    }
}