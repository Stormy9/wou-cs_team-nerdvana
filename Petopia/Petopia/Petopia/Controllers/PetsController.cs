using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Petopia.DAL;
using Petopia.Models;
using Petopia.Models.ViewModels;
using static Petopia.Models.ViewModels.PetGalleryViewModel;

namespace Petopia.Controllers
{
    public class PetsController : Controller
    {
        private PetopiaContext db = new PetopiaContext();

        //===============================================================================
        // GET: Pets
        public ActionResult Index()
        {
            var pets = db.Pets.Include(p => p.PetOwner);

            return View(pets.ToList());
        }

        //===============================================================================
        // GET: Pets/Profile/5                                  // THIS IS PET'S PROFILE!
        public ActionResult PetProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DAL.Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }

            //---------------------------------------------------------------------------
            // make pet's birthday a better format!                         it worked!
            var petsBday = pet.Birthdate;

            ViewBag.PetsBday = petsBday.ToString("MMMM dd, yyyy");

            //---------------------------------------------------------------------------
            // trying to pull in pet's owner's name.....                    it worked!
            var petsOwnerID = pet.PetOwnerID;

            var petOwnerID = db.PetOwners.Where(po => po.PetOwnerID == petsOwnerID)
                                         .Select(po => po.UserID)
                                         .FirstOrDefault();

            var petOwnerFirstName = db.PetopiaUsers.Where(poID => poID.UserID == petOwnerID)
                                                   .Select(pon => pon.FirstName)
                                                   .FirstOrDefault();

            var petOwnerLastName = db.PetopiaUsers.Where(poID => poID.UserID == petOwnerID)
                                                  .Select(pon => pon.LastName)
                                                  .FirstOrDefault();

            ViewBag.PetsOwnersFirstName = petOwnerFirstName;
            ViewBag.PetOwnersLastName = petOwnerLastName;

            //---------------------------------------------------------------------------
            // trying to pull in pet's general location (which is same as owners, duh!
            var petsGeneralLocation = db.PetopiaUsers.Where(poID => poID.UserID == petOwnerID)
                                                     .Select(pgl => pgl.GeneralLocation)
                                                     .FirstOrDefault();

            ViewBag.PetsGeneralLocation = petsGeneralLocation;

            //---------------------------------------------------------------------------
            // testing to find this Pet's owner -- 
            //        to ONLY show details/appts/editPet buttons to the Pet's owner!
            // find this Pet's Owner's ID
            var thisPetsOwnersID = db.Pets.Where(p => p.PetID == id)
                                          .Select(poID => poID.PetOwnerID)
                                          .FirstOrDefault();

            // now pull this Pet Owner's PetopiaUser ID
            var thisPetsOwnersPetopiaUserID = db.PetOwners.Where(pu => pu.PetOwnerID == thisPetsOwnersID)
                                                          .Select(puID => puID.UserID)
                                                          .FirstOrDefault();

            // now pull this PetopiaUser's ASPNetIdentityID
            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();

            // now pull the logged-in user's ID
            var loggedInUser = User.Identity.GetUserId();

            // so kinda backwards from the queries in the profile or care transaction controllers!
            ViewBag.thisPetsOwnersID = "This Pet's PetOwnerID: " + thisPetsOwnersID;
            ViewBag.thisPetsOwnersPetopiaUserID = "This Pet's Owner's PetopiaUserID: " 
                                                        + thisPetsOwnersPetopiaUserID;
            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;

            //---------------------------------------------------------------------------

            return View(pet);
        }

        //===============================================================================
        // GET: Pets/AddPet
        public ActionResult AddPet()
        {
            // pick list for rating?  like for 1 thru 5?
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating");

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View();
        }

        //-------------------------------------------------------------------------------
        // POST: Pets/AddPet
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details:  https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPet(PetPicViewModel model)
        {
            DAL.Pet pet = new DAL.Pet();

            if (ModelState.IsValid)
            {

                var identityID = User.Identity.GetUserId();
                var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                              .Select(x => x.UserID)
                                              .First();

                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID)
                                          .Select(x => x.PetOwnerID)
                                          .First();

                pet.PetName = model.PetName;
                pet.Species = model.Species;
                pet.Breed = model.Breed;
                pet.Gender = model.Gender;
                pet.Birthdate = model.Birthdate;
                pet.Weight = model.Weight;
                pet.HealthConcerns = model.HealthConcerns;
                pet.BehaviorConcerns = model.BehaviorConcerns;
                pet.PetAccess = model.PetAccess;
                pet.EmergencyContactName = model.EmergencyContactName;
                pet.EmergencyContactPhone = model.EmergencyContactPhone;
                pet.NeedsDetails = model.NeedsDetails;
                pet.PetCaption = model.PetCaption;
                pet.PetBio = model.PetBio;

                pet.PetOwnerID = ownerID;

                //For profile picture
                if (model.PetPhoto != null)
                {
                    if (model.PetPhoto.ContentLength > (4 * 1024 * 1024))
                    {
                        ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                        return View(model);
                    }

                    if (!(model.PetPhoto.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("CustomError", "Image must be in jpeg format.");
                        return View(model);
                    }

                    byte[] data = new byte[model.PetPhoto.ContentLength];

                    model.PetPhoto.InputStream.Read(data, 0, model.PetPhoto.ContentLength);

                    pet.PetPhoto = data;
                }

                db.Pets.Add(pet);
                db.SaveChanges();

                return RedirectToAction("PetProfile", new { id = pet.PetID });
            }

            // pick list for rating -- like 1 thru 5
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View(model);
        }

        //===============================================================================
        // GET: Pets/EditPet/5
        public ActionResult EditPet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DAL.Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }

            PetPicViewModel model = new PetPicViewModel();

            model.PetName = pet.PetName;
            model.Species = pet.Species;
            model.Breed = pet.Breed;
            model.Gender = pet.Gender;
            model.Birthdate = pet.Birthdate;
            model.Weight = pet.Weight;
            model.HealthConcerns = pet.HealthConcerns;
            model.BehaviorConcerns = pet.BehaviorConcerns;
            model.PetAccess = pet.PetAccess;
            model.EmergencyContactName = pet.EmergencyContactName;
            model.EmergencyContactPhone = pet.EmergencyContactPhone;
            model.NeedsDetails = pet.NeedsDetails;
            model.PetCaption = pet.PetCaption;
            model.PetBio = pet.PetBio;
            model.PetID = pet.PetID;


            // pick-list for rating -- 1 thru 5
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            //---------------------------------------------------------------------------
            // testing to find this Pet's owner -- 
            //        to ONLY show details/appts/editPet buttons to the Pet's owner!
            // find this Pet's Owner's ID
            var thisPetsOwnersID = db.Pets.Where(p => p.PetID == id)
                                          .Select(poID => poID.PetOwnerID)
                                          .FirstOrDefault();

            // now pull this Pet Owner's PetopiaUser ID
            var thisPetsOwnersPetopiaUserID = db.PetOwners.Where(pu => pu.PetOwnerID == thisPetsOwnersID)
                                                          .Select(puID => puID.UserID)
                                                          .FirstOrDefault();

            // now pull this PetopiaUser's ASPNetIdentityID
            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();

            // now pull the logged-in user's ID
            var loggedInUser = User.Identity.GetUserId();

            // so kinda backwards from the queries in the profile or care transaction controllers!
            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;

            //---------------------------------------------------------------------------

            return View(model);
        }
        //-------------------------------------------------------------------------------
        // POST: Pets/EditPet/5
        // To protect from overposting attacks, please enable the specific properties you
        // want to bind to; more details:  https://go.microsoft.com/fwlink/?LinkId=317598
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditPet(PetPicViewModel model)
        {
            DAL.Pet pet = new DAL.Pet();

            if (ModelState.IsValid)
            {
                var identityID = User.Identity.GetUserId();

                var loggedID = db.PetopiaUsers.Where(x => x.ASPNetIdentityID == identityID)
                                              .Select(x => x.UserID)
                                              .First();

                int ownerID = db.PetOwners.Where(x => x.UserID == loggedID)
                                          .Select(x => x.PetOwnerID)
                                          .First();


                pet.PetOwnerID = ownerID;

                pet.PetName = model.PetName;
                pet.Species = model.Species;
                pet.Breed = model.Breed;
                pet.Gender = model.Gender;
                pet.Birthdate = model.Birthdate;
                pet.PetCaption = model.PetCaption;
                pet.PetBio = model.PetBio;

                pet.Weight = model.Weight;
                pet.HealthConcerns = model.HealthConcerns;
                pet.BehaviorConcerns = model.BehaviorConcerns;
                pet.PetAccess = model.PetAccess;
                pet.EmergencyContactName = model.EmergencyContactName;
                pet.EmergencyContactPhone = model.EmergencyContactPhone;
                pet.NeedsDetails = model.NeedsDetails;

                pet.PetID = model.PetID;

                // pet profile picture 
                if (model.PetPhoto != null)
                {
                    if (model.PetPhoto.ContentLength > (4 * 1024 * 1024))
                    {
                        ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                        return View(model);
                    }

                    if (!(model.PetPhoto.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("CustomError", "Image must be in jpeg format.");
                        return View(model);
                    }

                    byte[] data = new byte[model.PetPhoto.ContentLength];

                    model.PetPhoto.InputStream.Read(data, 0, model.PetPhoto.ContentLength);

                    pet.PetPhoto = data;
                }
                else //If no pic was uploaded, we need to seed the current profile pic into our user
                {
                    pet.PetPhoto = db.Pets.Where(x => x.PetID == pet.PetID).Select(x => x.PetPhoto).FirstOrDefault();
                }
                //-----------------------------------------------------

                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("PetProfile", new { id = pet.PetID } );
            }

            // pick list for rating -- like 1 thru 5
            ViewBag.PetOwnerID = new SelectList(db.PetOwners, "PetOwnerID", "AverageRating", pet.PetOwnerID);

            // boy-girl-altered pick-list
            ViewBag.GenderList = genderSelectList;

            return View(model);
        }

        //===============================================================================
        // GET: Pets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DAL.Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }

            return View(pet);
        }
        //-------------------------------------------------------------------------------
        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DAL.Pet pet = db.Pets.Find(id);
            db.Pets.Remove(pet);
            db.SaveChanges();

            return RedirectToAction("ProfilePage", "Index");
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
        //------------ BOY/GIRL DROP-DOWN LIST IN 'CREATE (and edit) PET' ---------------
        //
        private IList<SelectListItem> genderSelectList = new List<SelectListItem>
        {
            new SelectListItem
                { Value = "girl", Text = "girl" },
            new SelectListItem
                { Value = "boy", Text = "boy" },
            new SelectListItem
                { Value = "boy (altered)", Text = "boy (altered)" },
            new SelectListItem
                { Value = "girl (altered)", Text = "girl (altered)" }
        };
        //===============================================================================
        public ActionResult MyPetDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DAL.Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }

            //---------------------------------------------------------------------------
            // testing to find this Pet's owner -- 
            //        to ONLY show details/appts/editPet buttons to the Pet's owner!
            // find this Pet's Owner's ID
            var thisPetsOwnersID = db.Pets.Where(p => p.PetID == id)
                                          .Select(poID => poID.PetOwnerID)
                                          .FirstOrDefault();

            // now pull this Pet Owner's PetopiaUser ID
            var thisPetsOwnersPetopiaUserID = db.PetOwners.Where(pu => pu.PetOwnerID == thisPetsOwnersID)
                                                          .Select(puID => puID.UserID)
                                                          .FirstOrDefault();

            // now pull this PetopiaUser's ASPNetIdentityID
            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();

            // now pull the logged-in user's ID
            var loggedInUser = User.Identity.GetUserId();

            // so kinda backwards from the queries in the profile or care transaction controllers!
            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;

            //---------------------------------------------------------------------------

            return View(pet);
        }
        //===============================================================================
        public ActionResult PetGallery(int? id)
        {
            var ThisPetsName = db.Pets.Where(pn => pn.PetID == id)
                                      .Select(pn => pn.PetName)
                                      .FirstOrDefault();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            PetGalleryViewModel petGal = new PetGalleryViewModel();

            petGal.CurrentPetID = id;

            petGal.PetGalleryList = db.PetGallery.Where(x => x.PetID == id)
                                                 .Select(n => new PetGalleryInfo
                                                 {
                                                     PetPicID = n.PetPicID,
                                                     PetID = n.PetID,
                                                     Comment = n.Comment
                                                 }).ToList();

            ViewBag.ImageCount = db.PetGallery.Where(x => x.PetID == id).Count();
            ViewBag.ThisPetID = id;
            ViewBag.ThisPetsName = ThisPetsName;

            //---------------------------------------------------------------------------
            // testing to find this Pet's owner -- 
            //        to ONLY show details/appts/editPet buttons to the Pet's owner!
            // find this Pet's Owner's ID
            var thisPetsOwnersID = db.Pets.Where(p => p.PetID == id)
                                          .Select(poID => poID.PetOwnerID)
                                          .FirstOrDefault();

            // now pull this Pet Owner's PetopiaUser ID
            var thisPetsOwnersPetopiaUserID = db.PetOwners.Where(pu => pu.PetOwnerID == thisPetsOwnersID)
                                                          .Select(puID => puID.UserID)
                                                          .FirstOrDefault();

            // now pull this PetopiaUser's ASPNetIdentityID
            var thisPetsOwnersASPNetIdentityID = db.PetopiaUsers.Where(pu => pu.UserID == thisPetsOwnersPetopiaUserID)
                                                                .Select(aspnetID => aspnetID.ASPNetIdentityID)
                                                                .FirstOrDefault();

            // now pull the logged-in user's ID
            var loggedInUser = User.Identity.GetUserId();

            // so kinda backwards from the queries in the profile or care transaction controllers!
            ViewBag.thisPetsOwnersASPNetIdentityID = thisPetsOwnersASPNetIdentityID;
            ViewBag.loggedInUser = loggedInUser;

            //---------------------------------------------------------------------------

            return View(petGal);
        }
        //===============================================================================
        //GET: Pet/PetGalleryCreate
        public ActionResult PetGalleryCreate(int? id)
        {
            PetGalleryViewModel PetG = new PetGalleryViewModel();
            PetG.CurrentPetID = db.Pets.Where(x => x.PetID == id)
                                       .Select(x => x.PetID)
                                       .First();

            return View(PetG);
        }
        //-------------------------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PetGalleryCreate(PetGalleryViewModel model)
        {
            PetGallery newPhoto = new PetGallery();

            if (model.GalleryPhoto != null)
            {
                if (model.GalleryPhoto.ContentLength > (4 * 1024 * 1024))
                {
                    ModelState.AddModelError("CustomError", "Image can not be lager than 4MB.");
                    return View(model);
                }

                if (!(model.GalleryPhoto.ContentType == "image/jpeg"))
                {
                    ModelState.AddModelError("CustomError", "Image must be in jpeg format.");
                    return View(model);
                }

                byte[] data = new byte[model.GalleryPhoto.ContentLength];

                model.GalleryPhoto.InputStream.Read(data, 0, model.GalleryPhoto.ContentLength);

                newPhoto.GalleryPhoto = data;
            }

            newPhoto.Comment = model.PhotoComment;
            newPhoto.PetID = model.CurrentPetID;

            db.PetGallery.Add(newPhoto);
            db.SaveChanges();

            return RedirectToAction("PetGallery", new { id = model.CurrentPetID });
        }
        //===============================================================================
        public ActionResult PetGalleryDelete(int id)
        {
            PetGallery petG = db.PetGallery.Find(id);
            int? petID = petG.PetID;
            db.PetGallery.Remove(petG);
            db.SaveChanges();

            return RedirectToAction("PetGallery", new { id =  petID});
        }
        //===============================================================================
    }
}
