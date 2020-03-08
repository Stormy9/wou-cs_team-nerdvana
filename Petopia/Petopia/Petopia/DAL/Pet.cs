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

        [StringLength(24)]
        public string Breed { get; set; }

        [Required]
        [StringLength(8)]
        public string Gender { get; set; }

        [StringLength(8)]
        public string Altered { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }

        [Column(TypeName = "date")]
        public DateTime Weight { get; set; }

        public string HealthConcerns { get; set; }

        public string BehaviorConcerns { get; set; }

        public string PetAccess { get; set; }

        [StringLength(45)]
        public string EmergencyContactName { get; set; }

        [StringLength(12)]
        public string EmergencyContactPhone { get; set; }

        public string NeedsDetails { get; set; }

        public string AccessInstructions { get; set; }


        // Foreign Key
        public int? PetOwnerID { get; set; }


        // Pull from other tables:
        public virtual PetOwner PetOwner { get; set; }


        // how the hell do we pull in Pet Owner first/last name??
        //public virtual PetopiaUser FirstName { get; set; }

        //public virtual PetopiaUser LastName { get; set; }
    }
}
