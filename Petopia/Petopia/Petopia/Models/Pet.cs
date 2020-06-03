namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pet")]
    public partial class Pet
    {
        //===============================================================================
        [DisplayName("PetID")]
        public int PetID { get; set; }

        //===============================================================================
        [Required(ErrorMessage = "please enter your pet's name")]
        [DisplayName("* Pet's Name:")]
        [StringLength(24)]
        public string PetName { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter your pet's species")]
        [DisplayName("* What Species?")]
        [StringLength(24)]
        public string Species { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter your pet's breed or mix")]
        [DisplayName("* And Breed?")]
        [StringLength(24)]
        public string Breed { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter your pet's gender")]
        [DisplayName("* Pet's Gender:")]
        [StringLength(18)]
        public string Gender { get; set; }

        //-------------------------------------------------------------------------------
        // we folded in 'Altered?' with Gender in a drop-down -- a lot smoother!       //
        //-------------------------------------------------------------------------------
        //
        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter your pet's birthday (or best guess)")]
        [Column(TypeName = "date")]     // gets rid of '12:00:00 AM' appendage
        //
        // adding this to the data definition gives us a date-picker in the view
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        // it also formats things correctly for going into our db..... yay!
        //
        [DisplayName("* Pet's Birthday:")]
        public DateTime Birthdate { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Weight (Pet's):")]
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
        [Required(ErrorMessage = "please give an emergency contact for your pet")]
        [DisplayName("* Pet's Emergency Contact Name:")]
        [StringLength(45)]
        public string EmergencyContactName { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please give an emergency contact for your pet")]
        [DisplayName("* Pet's Emergency Contact Number:")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "please enter phone number in requested format")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                            ErrorMessage = "please enter phone number as requested")]
        [StringLength(12)]
        public string EmergencyContactPhone { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("What My Pet Needs Done:")]
        public string NeedsDetails { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Profile Photo Caption:")]
        public string PetCaption { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("My Pet's Bio:")]
        public string PetBio { get; set; }

        //===============================================================================
        // FOREIGN KEY
        [DisplayName("OwnerID")]
        public int? PetOwnerID { get; set; }

        //===============================================================================
        // Pull from other tables:  (or is it?)
        public virtual PetOwner PetOwner { get; set; }

        //===============================================================================
    }
}
