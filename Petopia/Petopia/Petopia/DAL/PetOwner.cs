namespace Petopia.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PetOwner")]
    public partial class PetOwner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PetOwner()
        {
            Pets = new HashSet<Pet>();
        }

        public int PetOwnerID { get; set; }

        [StringLength(120)]
        public string AverageRating { get; set; }

        [Required]
        public string NeedsDetails { get; set; }

        [Required]
        public string AccessInstructions { get; set; }

        public int? UserID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pet> Pets { get; set; }

        public virtual PetopiaUser PetopiaUser { get; set; }
    }
}
