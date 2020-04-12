namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserBadge")]
    public partial class UserBadge
    {
        [Key]
        public int UserBadgeID { get; set; }

        public int? UserID { get; set; }

        public bool DogOwner { get; set; }
        public bool DogProvider { get; set; }
        public bool CatOwner { get; set; }
        public bool CatProvider { get; set; }
        public bool BirdOwner { get; set; }
        public bool BirdProvider { get; set; }

        //===============================================================================
    }
}