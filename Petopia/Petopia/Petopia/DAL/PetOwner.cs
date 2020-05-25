namespace Petopia.DAL
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
        //===============================================================================
        [Required]
        [DisplayName("PetOwnerID")]
        public int PetOwnerID { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(120)]
        [DisplayName("Owner Avg Rating")]
        public string AverageRating { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("My general pet care needs:")]
        public string GeneralNeeds { get; set; }

        //-------------------------------------------------------------------------------
        [Required]
        [DisplayName("How to access our home:")]
        public string HomeAccess { get; set; }

        //===============================================================================
        // FOREIGN KEY
        [DisplayName("PetopiaUserID")]
        public int? UserID { get; set; }

        //===============================================================================
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public PetOwner()
        {
            Pets = new HashSet<Pet>();
        }
        //-------------------------------------------------------------------------------
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<Pet> Pets { get; set; }

        //===============================================================================
        public virtual PetopiaUser PetopiaUser { get; set; }

        //===============================================================================
    }
}
