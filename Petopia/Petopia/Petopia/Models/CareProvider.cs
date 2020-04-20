namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CareProvider")]
    public partial class CareProvider
    {
        //===============================================================================
        public int CareProviderID { get; set; }

        //===============================================================================
        [StringLength(120)]
        public string AverageRating { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("My Pet Care Experience:")]
        public string ExperienceDetails { get; set; }

        //===============================================================================
        // FOREIGN KEY
        public int? UserID { get; set; }

        //===============================================================================
        public virtual PetopiaUser PetopiaUser { get; set; }

        //===============================================================================
    }
}
