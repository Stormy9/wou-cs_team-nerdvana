namespace Petopia.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pet")]
    public partial class Pet
    {
        public int PetID { get; set; }

        [Required]
        [StringLength(24)]
        public string PetName { get; set; }

        [Required]
        [StringLength(24)]
        public string Species { get; set; }

        [Required]
        [StringLength(24)]
        public string Breed { get; set; }

        [Required]
        [StringLength(8)]
        public string Gender { get; set; }

        [Required]
        [StringLength(8)]
        public string Altered { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }

        [Column(TypeName = "date")]
        public DateTime Weight { get; set; }

        [Required]
        public string HealthConcerns { get; set; }

        [Required]
        public string BehaviorConcerns { get; set; }

        [Required]
        public string PetAccess { get; set; }

        [Required]
        [StringLength(45)]
        public string EmergencyContactName { get; set; }

        [Required]
        [StringLength(12)]
        public string EmergencyContactPhone { get; set; }

        [Required]
        public string NeedsDetails { get; set; }

        [Required]
        public string AccessInstructions { get; set; }

        public int? PetOwnerID { get; set; }

        public virtual PetOwner PetOwner { get; set; }
    }
}
