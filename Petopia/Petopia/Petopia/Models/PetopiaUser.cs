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
        //===============================================================================
        [Key]
        public int UserID { get; set; }

        //===============================================================================
        [StringLength(120)]
        public string UserName { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(50)]
        public string Password { get; set; }

        //===============================================================================

        [Required(ErrorMessage = "please enter your first name")]
        [DisplayName("* First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter your last name")]
        [DisplayName("* Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        //===============================================================================
        [StringLength(128)]
        public string ASPNetIdentityID { get; set; }

        //-------------------------------------------------------------------------------
        public bool IsOwner { get; set; }

        //-------------------------------------------------------------------------------
        public bool IsProvider { get; set; }

        //===============================================================================
        [Required]
        [DataType(DataType.PhoneNumber, 
                           ErrorMessage = "please enter your phone number as requested")]
        [DisplayName("* Main Phone #")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        [StringLength(12)]
        public string MainPhone { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(12)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "please enter a valid phone number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        [DisplayName("Alternate Phone #")]
        public string AltPhone { get; set; }

        //===============================================================================
        [Required(ErrorMessage = "please enter your residential street address")]
        [DisplayName("* Residential Street Address")]
        [StringLength(50)]
        public string ResAddress01 { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Address, 2nd Line (if you have one)")]
        [StringLength(50)]
        public string ResAddress02 { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter your residential city")]
        [DisplayName("* City")]
        [StringLength(20)]
        public string ResCity { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter 2-character state")]
        [DisplayName("* State")]
        [StringLength(2)]
        public string ResState { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter 5-digit zipcode")]
        [DisplayName("* ZipCode")]
        [StringLength(5)]
        public string ResZipcode { get; set; }

        //===============================================================================
        [DisplayName("Upload your Profile Photo!")]
        public byte[] ProfilePhoto { get; set; }

        //------------------------------------------------------------------------------
        [StringLength(72)]
        [DisplayName("Caption for your profile pic:")]
        public string UserCaption { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(72)]
        [DisplayName("A tagline to go under your name:")]
        public string Tagline { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Your Bio - some fun stuff about you:")]
        public string UserBio { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(72)]
        [DisplayName("Your General Location:")]
        public string GeneralLocation { get; set; }

        //===============================================================================

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CareProvider> CareProviders { get; set; }

        //-------------------------------------------------------------------------------
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PetOwner> PetOwners { get; set; }

        //===============================================================================
    }
}
