using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class PetopiaUserLoginViewModel
    {
        //NEED TO MAKE THIS DUMB VIEWMODEL JUST SO WE CAN ADD PROFILE PICTURES ON REGISTER
        //everything is the same as our PetopiaUser model just with the profile picture changed to the right type.
        public int UserID { get; set; }

        //===============================================================================
        [StringLength(120)]
        public string UserName { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(50)]
        public string Password { get; set; }

        //===============================================================================

        [Required]
        [DisplayName("First Name*")]
        [StringLength(50)]
        public string FirstName { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("Last Name*")]
        [StringLength(50)]
        public string LastName { get; set; }

        //===============================================================================
        [StringLength(128)]
        public string ASPNetIdentityID { get; set; }

        //-------------------------------------------------------------------------------
        public bool IsOwner { get; set; }

        //-------------------------------------------------------------------------------
        public bool IsProvider { get; set; }

        //===============================================================================
        [Required]
        [DisplayName("Main Phone #*")]
        [StringLength(50)]
        public string MainPhone { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(50)]
        [DisplayName("Alternate Phone #")]
        public string AltPhone { get; set; }

        //===============================================================================

        [Required]
        [DisplayName("Residential Street Address*")]
        [StringLength(50)]
        public string ResAddress01 { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Address, 2nd Line (if you have one)")]
        [StringLength(50)]
        public string ResAddress02 { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("City*")]
        [StringLength(50)]
        public string ResCity { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("State*")]
        [StringLength(50)]
        public string ResState { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("ZipCode*")]
        [StringLength(24)]
        public string ResZipcode { get; set; }

        //===============================================================================
        [DisplayName("Upload your Profile Photo!")]
        public HttpPostedFileBase ProfilePhoto { get; set; }

        //------------------------------------------------------------------------------
        [StringLength(72)]
        [DisplayName("Caption for your profile pic:")]
        public string UserCaption { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(72)]
        [DisplayName("A tagline to go under your name:")]
        public string Tagline { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Your Bio - some fun stuff about you:")]
        public string UserBio { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(72)]
        [DisplayName("Your General Location:")]
        public string GeneralLocation { get; set; }
    }
}