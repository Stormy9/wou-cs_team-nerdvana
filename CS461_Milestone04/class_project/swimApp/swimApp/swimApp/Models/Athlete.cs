namespace swimApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Athlete")]
    public partial class Athlete
    {
        [Key]
        public int AID { get; set; }

        [Required]
        [StringLength(1028)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(1028)]
        public string LastName { get; set; }

        [Required]
        [StringLength(1028)]
        public string Gender { get; set; }
    }
}
