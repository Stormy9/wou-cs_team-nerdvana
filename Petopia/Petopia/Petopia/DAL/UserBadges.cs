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
        [DisplayName("Bird ")]
        public bool BirdOwner { get; set; }
        [DisplayName("Bird ")]
        public bool BirdProvider { get; set; }
        [DisplayName("Fish ")]
        public bool FishOwner { get; set; }
        [DisplayName("Fish ")]
        public bool FishProvider { get; set; }
        [DisplayName("Horse ")]
        public bool HorseOwner { get; set; }
        [DisplayName("Horse ")]
        public bool HorseProvider { get; set; }
        [DisplayName("Livestock ")]
        public bool LivestockOwner { get; set; }
        [DisplayName("Livestock ")]
        public bool LivestockProvider { get; set; }
        [DisplayName("Rabbit ")]
        public bool RabbitOwner { get; set; }
        [DisplayName("Rabbit ")]
        public bool RabbitProvider { get; set; }
        [DisplayName("Reptile ")]
        public bool ReptileOwner { get; set; }
        [DisplayName("Reptile ")]
        public bool ReptileProvider { get; set; }
        [DisplayName("Small rodents ")]
        public bool RodentOwner { get; set; }
        [DisplayName("Small rodents ")]
        public bool RodentProvider { get; set; }
        [DisplayName("Other ")]
        public bool OtherOwner { get; set; }
        [DisplayName("Other ")]
        public bool OtherProvider { get; set; }


        public virtual PetopiaUser PetopiaUser { get; set; }
    }
}