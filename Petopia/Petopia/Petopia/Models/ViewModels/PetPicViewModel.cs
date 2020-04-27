using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class PetPicViewModel
    {
        public int PetID { get; set; }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        [Required]
        [DisplayName("Pet's Name:")]
        [StringLength(24)]
        public string PetName { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("What Species?")]
        [StringLength(24)]
        public string Species { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("And Breed?")]
        [StringLength(24)]
        public string Breed { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("Pet's Gender:")]
        [StringLength(18)]
        public string Gender { get; set; }

        //-------------------------------------------------------------------------------
        // we folded in 'Altered?' with Gender in a drop-down -- a lot smoother!       //
        //-------------------------------------------------------------------------------
        //
        //-------------------------------------------------------------------------------
        [Column(TypeName = "date")]     // gets rid of '12:00:00 AM' appendage
        //
        // adding this to the data definition gives us a date-picker in the view
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        // it also formats things correctly for going into our db..... yay!
        //
        [DisplayName("Pet's Birthday:")]
        public DateTime Birthdate { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Weight (Pet's):")]
        [StringLength(3)]
        public string Weight { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("My Pet's Health Notes:")]
        public string HealthConcerns { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("My Pet's Behavior Notes:")]
        public string BehaviorConcerns { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("How To Access My Pet:")]
        public string PetAccess { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet's Emergency Contact Name:")]
        [StringLength(45)]
        public string EmergencyContactName { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet's Emergency Contact Number:")]
        [StringLength(12)]
        public string EmergencyContactPhone { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("What My Pet Needs Done:")]
        public string NeedsDetails { get; set; }

        //------------------------------------------------------------------------------
        [DisplayName("Pet pic!")]
        public HttpPostedFileBase PetPhoto { get; set; }


        [DisplayName("Profile pic caption:")]
        [StringLength(72)]
        public string PetCaption { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("My Pet's Bio:")]
        public string PetBio { get; set; }

        //===============================================================================
        // to put owner's name on pet's profile!
        public string FirstName { get; set; }

        public string LastName { get; set; }
        //===============================================================================
    }
}