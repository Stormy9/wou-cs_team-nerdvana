using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Petopia.Models.ViewModels
{
    public class CareTransactionViewModel
    {
        //===============================================================================
        //===============================================================================
        // MAIN stuff from PetopiaUsers table:
        public int UserId { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Owner's first name:")]
        public string FirstName { get; set; }

        [DisplayName("Owner's last name:")]
        public string LastName { get; set; }

        //-------------------------------------------------------------------------------
        public bool IsOwner { get; set; }
        public bool IsProvider { get; set; }

        //===============================================================================
        // Background\Private Account Info from PetopiaUsers table:
        [DisplayName("Main Phone #:")]
        public string MainPhone { get; set; }

        [DisplayName("Alternate Phone #:")]
        public string AltPhone { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Residential Street Address:")]
        public string ResAddress01 { get; set; }

        [DisplayName("Address, 2nd line:")]
        public string ResAddress02 { get; set; }

        [DisplayName("City:")]
        public string ResCity { get; set; }

        [DisplayName("State:")]
        public string ResState { get; set; }

        [DisplayName("ZipCode:")]
        public string ResZipcode { get; set; }

        //===============================================================================
        //
        //
        //===============================================================================
        // main thing from Pet table:
        [DisplayName("Pet's name:")]
        public string PetName { get; set; }
        
        //===============================================================================
        // the "background\private details" from Pet table:
        [DisplayName("Pet's species:")]
        public string Species { get; set; }

        [DisplayName("Pet's breed:")]
        public string Breed { get; set; }

        [DisplayName("Pet's gender:")]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Pet's Birthday:")]
        public DateTime Birthday { get; set; }

        [DisplayName("Pet's weight:")]
        public string Weight { get; set; }

        [DisplayName("Where & how to access pet:")]
        public string PetAccess { get; set; }

        [DisplayName("Pet's general care needs:")]
        public string NeedsDetails { get; set; }

        [DisplayName("Pet's health notes:")]
        public string HealthConcerns { get; set; }

        [DisplayName("Pet's behavior notes:")]
        public string BehaviorConcerns { get; set; }

        [DisplayName("Pet's emergency contact person:")]
        public string EmergencyContactName { get; set; }

        [DisplayName("Pet's emergency contact #:")]
        public string EmergencyContactPhone { get; set; }

        //-------------------------------------------------------------------------------
        // we will need a Pet List to make a drop-down for a User's Pets -- 
        //   -- for the owner to choose which Pet they are booking care for
        //
        //    (borrowed\pared down from Corrin's ProfileViewModel!)
        // Below here is for Pet drop-down list -- choose Pet to get care for 
        public class PetInfo
        {
            public int PetID { get; set; }
            public string PetName { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<PetInfo> PetList { get; set; }
        
        //===============================================================================
        //
        //
        //===============================================================================
        // the "background\private info" from PetOwner table:
        [DisplayName("How to access our Home:")]
        public string HomeAccess { get; set; }

        [DisplayName("My General Pet Care Needs:")]
        public string GeneralNeeds { get; set; }

        //===============================================================================
        // the "background info" from the CareProvider table:

        // don't think we need any for this part of things?     well, maybe?
        [DisplayName("My Pet Care Experience:")]
        public string ExperienceDetails { get; set; }
        //===============================================================================
        //
        //
        //===============================================================================
        // now the stuff from the actual CareTransactions table!
        public int TransactionID { get; set; }

        //-------------------------------------------------------------------------------
        [Column(TypeName = "date")]     // gets rid of '12:00:00 AM' appendage
        //
        // adding this to the data definition gives us a date-picker in the view
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        // it also formats things correctly for going into our db..... yay!
        //
        [DisplayName("Start Date:")]
        public DateTime StartDate { get; set; }


        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("End Date:")]
        public DateTime EndDate { get; set; }

        //-------------------------------------------------------------------------------
        [DataType(DataType.Time)]
        [DisplayName("Start Time:")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayName("End Time:")]
        public DateTime EndTime { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("What my Pet needs for this appointment:")]
        public string NeededThisVisit { get; set; }

        [DisplayName("What I did for your Pet this appointment:")]
        public string CareProvided { get; set; }

        [DisplayName("Care Report -- Details:")]
        public string CareReport { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet Carer's Fee:")]
        public decimal Charge { get; set; }

        [DisplayName("Tip?")]
        public decimal Tip { get; set; }

        //-------------------------------------------------------------------------------
        [Range(1,5)]
        [DisplayName("Pet Carer Rating:")]
        public int PC_Rating { get; set; }

        [DisplayName("Pet Carer Comments:  (from Pet Owner!)")]
        public string PC_Comments { get; set; }

        //-------------------------------------------------------------------------------
        [Range(1, 5)]
        [DisplayName("Pet Owner Rating:")]
        public int PO_Rating { get; set; }

        [DisplayName("Pet Owner Comments:  (from Pet Carer!)")]
        public string PO_Comments { get; set; }

        //===============================================================================
        // FOREIGN KEYS FROM CareTransactions TABLE
        
        public int PetOwnerID { get; set; }

        [DisplayName("Which Pet Carer?")]
        public int CareProviderID { get; set; }

        [DisplayName("Which Pet?")]
        public int PetID { get; set; }

        //===============================================================================
        //===============================================================================
    }
}