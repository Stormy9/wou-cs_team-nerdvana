using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class ProfileViewModel
    {
        //everything from PetopiaUser
        public int UserID { get; set; }

        [DisplayName("First Name:")]
        public string FirstName { get; set; }

        [DisplayName("Last Name:")]
        public string LastName { get; set; }

        //===============================================================================
        //Need this to know if we should show owner info on page
        public bool IsOwner { get; set; } 

        //Need this to know if we should show provider info on page
        public bool IsProvider { get; set; }
        //===============================================================================

        [DisplayName("State:")]
        public string ResState { get; set; }

        [DisplayName("Profile Pic:")]
        public byte[] ProfilePhoto { get; set; } //needSET

        public HttpPostedFileBase UserProfilePicture { get; set; }

        //===============================================================================
        //Average rating named changed because it shares name with other average rating
        //Everything from CareProvider

        [DisplayName("Avg Rating:")]
        public string ProviderAverageRating { get; set; }

        [DisplayName("Pet Care Experience:")]
        public string ExperienceDetails { get; set; }


        //===============================================================================
        //Everything from PetOwner

        [DisplayName("Avg Rating:")]
        public string OwnerAverageRating { get; set; }

        [DisplayName("Pet Care Needs:")]
        public string NeedsDetails { get; set; }

        [DisplayName("Access Instructions:")]
        public string AccessInstructions { get; set; }


        //===============================================================================
        //Everything below here won't be included in the profile page 
        //      but can still be edited
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

        [DisplayName("Zips:")]
        public string ResZipcode { get; set; }


        //===============================================================================
        //Below here is for Pet Profiles
        public class PetInfo
        {
            public int PetID { get; set; }
            public string PetName { get; set; }
            public string Species { get; set; }
            public string Gender { get; set; }
        }
        public List<PetInfo> PetList { get; set; }

    }
}