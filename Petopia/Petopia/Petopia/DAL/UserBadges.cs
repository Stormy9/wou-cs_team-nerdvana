using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Petopia.DAL
{

    [Table("UserBadge")]
    public partial class UserBadge
    {
        [Required]
        public int UserBadgeID { get; set; }

        public int? UserID { get; set; }
        [DisplayName("Dog ")]
        public bool DogOwner { get; set; }
        [DisplayName("Dog ")]
        public bool DogProvider { get; set; }
        [DisplayName("Cat ")]
        public bool CatOwner { get; set; }
        [DisplayName("Cat ")]
        public bool CatProvider { get; set; }
        [DisplayName("Bunny ")]
        public bool BirdOwner { get; set; }
        [DisplayName("Bunny ")]
        public bool BirdProvider { get; set; }

        public virtual PetopiaUser PetopiaUser { get; set; }
    }
}