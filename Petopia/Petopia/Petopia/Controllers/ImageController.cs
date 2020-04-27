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
        public ActionResult ShowPet(int id)
        {
            var imageData = db.Pets.Where(x => x.PetID == id).Select(x => x.PetPhoto).First();
            return File(imageData, "image/jpeg");
        }
        public ActionResult ShowPetGallery(int id)
        {
            var imageData = db.PetGallery.Where(x => x.PetPicID == id).Select(x => x.GalleryPhoto).FirstOrDefault();
            if (imageData == null)
            {
                return null;
            }
            else
            {
                return File(imageData, "image/jpeg");
            }
        }
    }
}