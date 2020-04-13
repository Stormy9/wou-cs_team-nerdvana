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
        public int PetID { get; set; }

        //===============================================================================
        [Required]
        [DisplayName("Pet Name:")]
        [StringLength(24)]
        public string PetName { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("Species:")]
        [StringLength(24)]
        public string Species { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Breed:")]
        [StringLength(24)]
        public string Breed { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("Gender:")]
        [StringLength(8)]
        public string Gender { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Altered?")]
        [StringLength(8)]
        public string Altered { get; set; }

        //-------------------------------------------------------------------------------
        [Column(TypeName = "date")]     // gets rid of '12:00:00 AM' appendage
        //
        // adding this to the data definition gives me a date-picker in the view
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        // it also formats things correctly for going into our db..... yay!
        //
        [DisplayName("Pet's Birthday:")]
        public DateTime Birthdate { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Weight:")]
        public DateTime Weight { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Health Concerns:")]
        public string HealthConcerns { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Behavior Concerns:")]
        public string BehaviorConcerns { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("How to access Pet:")]
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
        [DisplayName("Describe Pet's Needs:")]
        public string NeedsDetails { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("How to access home:")]
        public string AccessInstructions { get; set; }

        //===============================================================================
        // FOREIGN KEY
        public int? PetOwnerID { get; set; }

        //===============================================================================
    }
}
