namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Owner")]
    public partial class Owner
    {
        [StringLength(120)]
        public string OwnerID { get; set; }

        [StringLength(120)]
        public string Username { get; set; }

        [Required]
        [DisplayName("First Name:")]
        [StringLength(120)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name:")]
        [StringLength(120)]
        public string LastName { get; set; }
    }
}
