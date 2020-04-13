using System;
using System.Collections.Generic;
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

        public bool DogOwner { get; set; }
        public bool DogProvider { get; set; }
        public bool CatOwner { get; set; }
        public bool CatProvider { get; set; }
        public bool BirdOwner { get; set; }
        public bool BirdProvider { get; set; }

        public virtual PetopiaUser PetopiaUser { get; set; }
    }
}