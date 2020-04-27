using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class AdminPanelViewModel
    {
        public class AdminPetopiaUser {
            public int UserID { get; set; }

            [DisplayName("First Name:")]
            public string FirstName { get; set; }

            [DisplayName("Last Name:")]
            public string LastName { get; set; }

            [DisplayName("Identity hash:")]
            public string ASPNetIdentityID { get; set; }


            //-------------------------------------------------------------------------------
            //Need this to know if we should show owner info on page
            public bool IsOwner { get; set; }


            //Need this to know if we should show provider info on page
            public bool IsProvider { get; set; }

            //-------------------------------------------------------------------------------

            [DisplayName("Profile Pic:")]
            public byte[] ProfilePhoto { get; set; }

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
        }

        public List<AdminPetopiaUser> UserList { get; set; }
        
    }
}