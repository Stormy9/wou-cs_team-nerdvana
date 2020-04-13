namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PetOwner")]
    public partial class PetOwner
    {
        public int PetOwnerID { get; set; }


        //===============================================================================
        [DisplayName("Average Rating")]
        [StringLength(120)]
        public string AverageRating { get; set; }


        //===============================================================================
        [Required]
        [DisplayName("My Pet Care Needs:")]
        public string NeedsDetails { get; set; }


        [Required]
        [DisplayName("How To Access Home:")]
        public string AccessInstructions { get; set; }


        //===============================================================================
        // FOREIGN KEY
        public int? UserID { get; set; }
    }
}
