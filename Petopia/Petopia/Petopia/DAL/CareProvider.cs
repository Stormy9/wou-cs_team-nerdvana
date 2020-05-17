namespace Petopia.DAL
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
        [DisplayName("Provider Avg Rating")]
        [StringLength(120)]
        public string AverageRating { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Experience\\Resume")]
        [Required]
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
