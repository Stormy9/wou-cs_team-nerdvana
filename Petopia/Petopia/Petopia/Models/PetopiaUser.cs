namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PetopiaUser
    {
        [Key]
        public int UserID { get; set; }


        //===============================================================================
        [StringLength(120)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Password { get; set; }
        //===============================================================================


        [Required]
        [DisplayName("First Name*")]
        [StringLength(50)]
        public string FirstName { get; set; }


        [Required]
        [DisplayName("Last Name*")]
        [StringLength(50)]
        public string LastName { get; set; }


        //===============================================================================
        [StringLength(128)]
        public string ASPNetIdentityID { get; set; }

        public bool IsOwner { get; set; }

        public bool IsProvider { get; set; }
        //===============================================================================


        [Required]
        [DisplayName("Phone Number*")]
        [StringLength(50)]
        public string MainPhone { get; set; }


        [StringLength(50)]
        [DisplayName("Secondary Phone Number")]
        public string AltPhone { get; set; }

        //===============================================================================


        [Required]
        [DisplayName("Residential Address*")]
        [StringLength(50)]
        public string ResAddress01 { get; set; }


        [DisplayName("Extra Address")]
        [StringLength(50)]
        public string ResAddress02 { get; set; }


        [Required]
        [DisplayName("City*")]
        [StringLength(50)]
        public string ResCity { get; set; }

        [Required]
        [DisplayName("State*")]
        [StringLength(50)]
        public string ResState { get; set; }

        [Required]
        [DisplayName("Zip*")]
        [StringLength(24)]
        public string ResZipcode { get; set; }
        //===============================================================================


        [DisplayName("Profile Pic")]
        public byte[] ProfilePhoto { get; set; }
        //===============================================================================
    }
}
