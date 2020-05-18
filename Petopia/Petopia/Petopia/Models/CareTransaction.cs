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
        [DisplayName("Start Date:")]
        //
        // adding this to the data definition gives me a date-picker in the view
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        // it also formats things correctly for going into our db..... yay!
        //
        public DateTime StartDate { get; set; }

        //-------------------------------------------------------------
        [Column(TypeName = "date")]
        [DisplayName("End Date:")]
        //
        // to get our date-picker:
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        //
        public DateTime EndDate { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Start Time:")]
        [DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        //-------------------------------------------------------------
        [DisplayName("End Time:")]
        [DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        //===============================================================================
        [DisplayName("Pet Care instructions for this visit:")]
        public string NeededThisVisit { get; set; }

        //-------------------------------------------------------------
        [DisplayName("Pet Care recap:")]
        public string CareProvided { get; set; }

        //-------------------------------------------------------------
        [DisplayName("Full Pet Care Report -- Details:")]
        public string CareReport { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet Carer's fee for this visit:")]
        public float? Charge { get; set; }

        [DisplayName("Tip? (optional)")]
        public float? Tip { get; set; }

        //===============================================================================
        [DisplayName("Pet Carer Rating:")]
        public int? PC_Rating { get; set; }

        [DisplayName("Comments on Pet Carer:  (from Pet Owner!)")]
        public string PC_Comments { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet Owner Rating:")]
        public int? PO_Rating { get; set; }

        [DisplayName("Comments on Pet Owner:  (from Pet Carer!)")]
        public string PO_Comments { get; set; }

        //===============================================================================
        // booleans for status of appointments
        [DisplayName("Pending")]
        public bool Pending { get; set; }

        [DisplayName("Confirmed")]
        public bool Confirmed { get; set; }

        [DisplayName("Completed - Pet Owner")]
        public bool Completed_PO { get; set; }

        [DisplayName("Completed - Pet Carer")]
        public bool Completed_CP { get; set; }

        //===============================================================================
        // FOREIGN KEYS
        //
        [DisplayName("Pet's Owner:")]
        public int PetOwnerID { get; set; }

        [DisplayName("Which Pet Carer?")]
        public int CareProviderID { get; set; }

        [DisplayName("Which Pet?")]
        public int PetID { get; set; }

        //===============================================================================
    }
}
