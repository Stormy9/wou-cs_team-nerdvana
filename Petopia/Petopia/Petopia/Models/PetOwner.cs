namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PetOwner")]
    public partial class PetOwner
    {
        public int PetOwnerID { get; set; }

        [StringLength(120)]
        public string AverageRating { get; set; }

        [Required]
        public string NeedsDetails { get; set; }

        [Required]
        public string AccessInstructions { get; set; }

        public int? UserID { get; set; }
    }
}
