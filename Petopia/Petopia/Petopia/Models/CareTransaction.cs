namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CareTransaction")]
    public partial class CareTransaction
    {
        //===============================================================================
        [Key]
        public int TransactionID { get; set; }

        //===============================================================================
        [Column(TypeName = "date")]     // gets rid of '12:00:00 AM' appendage
        [DisplayName("Start Date")]
        //
        // adding this to the data definition gives me a date-picker in the view
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        // it also formats things correctly for going into our db..... yay!
        //
        public DateTime StartDate { get; set; }

        //-------------------------------------------------------------------------------
        [Column(TypeName = "date")] 
        [DisplayName("End Date")]
        //
        // to get our date-picker:
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //
        public DateTime EndDate { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        //-------------------------------------------------------------------------------
        [StringLength(90)]
        [DisplayName("Care Provided")]
        public string CareProvided { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Care Report")]
        public string CareReport { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Fees Charged")]
        [Column(TypeName = "money")]
        public decimal? Charge { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Tip (optional)")]
        [Column(TypeName = "money")]
        public decimal? Tip { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet Carer Rating")]
        public int? PC_Rating { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Comments on Pet Carer (from Pet Owner)")]
        public string PC_Comments { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet Owner Rating")]
        public int? PO_Rating { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Comments on Pet Owner (from Pet Carer)")]
        public string PO_Comments { get; set; }

        //===============================================================================
        // FOREIGN KEYS
        public int PetOwnerID { get; set; }

        //-------------------------------------------------------------------------------
        public int CareProviderID { get; set; }

        //-------------------------------------------------------------------------------
        public int PetID { get; set; }

        //===============================================================================
    }
}
