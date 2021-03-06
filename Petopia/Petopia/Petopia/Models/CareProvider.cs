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
        [DisplayName("ProviderID")]
        public int CareProviderID { get; set; }

        //===============================================================================
        [DisplayName("Pet Carer Average Rating")]
        public decimal? AverageRating { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("My Pet Care Experience:")]
        public string ExperienceDetails { get; set; }

        //===============================================================================
        // FOREIGN KEY
        [DisplayName("PetopiaUserID")]
        public int? UserID { get; set; }

        //===============================================================================
        public virtual PetopiaUser PetopiaUser { get; set; }

        //===============================================================================
    }
}
