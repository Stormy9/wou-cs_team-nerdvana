using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class ProfileViewModel
    {
        //===============================================================================
        //===============================================================================
        //everything from PetopiaUser
        public int UserID { get; set; }

        [DisplayName("First Name:")]
        public string FirstName { get; set; }

        [DisplayName("Last Name:")]
        public string LastName { get; set; }

        //-------------------------------------------------------------------------------
        //Need this to know if we should show owner info on page
        public bool IsOwner { get; set; } 


        //Need this to know if we should show provider info on page
        public bool IsProvider { get; set; }

        //-------------------------------------------------------------------------------

        [DisplayName("Profile Pic:")]
        public byte[] ProfilePhoto { get; set; } //needSET

        public HttpPostedFileBase UserProfilePicture { get; set; }


        [DisplayName("Caption for your profile pic:")]
        public string UserCaption { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Your General Location:")]
        public string GeneralLocation { get; set; }


        [DisplayName("Your Bio - some fun stuff about you:")]
        public string UserBio { get; set; }


        [DisplayName("A tagline to go under your name:")]
        public string Tagline { get; set; }

        //-------------------------------------------------------------------------------
        //Average rating name changed because it shares name with other average rating
        //
        //Everything from CareProvider

        [DisplayName("Avg Rating:")]
        public string ProviderAverageRating { get; set; }

        [DisplayName("Pet Care Experience:")]
        public string ExperienceDetails { get; set; }

        //===============================================================================
        //===============================================================================
        //Everything from PetOwner

        [DisplayName("Avg Rating:")]
        public string OwnerAverageRating { get; set; }

        [DisplayName("My Pet Care Needs:")]
        public string GeneralNeeds { get; set; }

        [DisplayName("How To Access Our Home:")]
        public string HomeAccess { get; set; }

        //===============================================================================
        //===============================================================================
        //Everything below here won't be included in the profile page 
        //      but can still be edited                                  from PetopiaUser
        [DisplayName("Main Phone:")]
        public string MainPhone { get; set; }

        [DisplayName("Alt Phone:")]
        public string AltPhone { get; set; }

        [DisplayName("Address:")]
        public string ResAddress01 { get; set; }

        [DisplayName("Address:")]
        public string ResAddress02 { get; set; }

        [DisplayName("City:")]
        public string ResCity { get; set; }

        [DisplayName("State:")]
        public string ResState { get; set; }

        [DisplayName("Zip:")]
        public string ResZipcode { get; set; }

        //===============================================================================
        //===============================================================================
        //Below here is for Pet Profiles  (for the Pet Cards on Owner pages right?)s
        public class PetInfo
        {
            public int PetID { get; set; }
            public string PetName { get; set; }
            public string Species { get; set; }
            public string Breed { get; set; }
            public string Gender { get; set; }

            [Column(TypeName = "date")]
            // that ^ is supposed to get rid of the time appendage on the Pet b-days on
            // the 'Pet Cards' on Owner profiles..... but, it isn't for some dumb reason
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
            public DateTime Birthdate { get; set; }
        }
        public List<PetInfo> PetList { get; set; }

        //===============================================================================
    }
}