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
        //                                                          PetopiaUsers table
        //===============================================================================
        // MAIN stuff from PetopiaUsers table:
        public int UserId { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("First name:")]                                // PetopiaUsers
        public string FirstName { get; set; }

        [DisplayName("Last name:")]
        public string LastName { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet Owner?")]                                 // PetopiaUsers
        public bool IsOwner { get; set; }

        [DisplayName("Pet Carer?")]
        public bool IsProvider { get; set; }

        //===============================================================================
        // Background\Private Account Info from                     PetopiaUsers table
        [DisplayName("Main Phone #:")]
        public string MainPhone { get; set; }

        [DisplayName("Alternate Phone #:")]
        public string AltPhone { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Residential Street Address:")]                    // PetopiaUsers
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
        //===============================================================================
        //                                                                  Pets table
        //===============================================================================
        // main things from Pet table:
        public int PetID { get; set; }

        [DisplayName("Pet's name:")]
        public string PetName { get; set; }
        
        //===============================================================================
        // the "background\private details" from                            Pet table:
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

        //===============================================================================
        // we will need a Pet List to make a drop-down for a User's Pets -- 
        //   -- for the owner to choose which Pet they are booking care for
        //
        //    (borrowed\pared down from Corrin's ProfileViewModel!)
        // Below here is for Pet drop-down list 
        //        -- choose which of your Pets to get care for (since you can have many!)
        //    ALSO -- to display Pet Carer's names in Appointment Listing Views
        public class PetNames
        {
            public int PetID { get; set; }

            public string PetName { get; set; }

            public bool PetIsChecked { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<PetNames> PetNameList { get; set; }
        
        //===============================================================================
        //===============================================================================
        //                                                              PetOwners Table
        //===============================================================================
        // the "background\private info" from PetOwner table:
        // 
        public int PetOwnerID { get; set; }

        // is there anything we can do with this?
        [DisplayName("Pet's Owner:")]
        public string PetOwnerName { get; set; }

        [DisplayName("How to access our Home:")]
        public string HomeAccess { get; set; }

        [DisplayName("My General Pet Care Needs:")]
        public string GeneralNeeds { get; set; }

        // foreign key???
        public int PO_UserID { get; set; }

        //===============================================================================
        //===============================================================================
        //                                                          CareProvider table
        //===============================================================================
        // the "background info" from the CareProvider table:

        public int CareProviderID { get; set; }

        // is there anything we can do with this?
        [DisplayName("Pet Carer:")]
        public string PetCarerName { get; set; }

        // don't think we need any for this part of things?     well, maybe?
        [DisplayName("My Pet Care Experience:")]
        public string ExperienceDetails { get; set; }

        // foreign key???
        public int CP_UserID { get; set; }

        //===============================================================================
        // we will need a List of Care Providers, to make a drop-down list of them,
        //   for a Users to select one when they book a pet care appointmentPets -- 
        //
        //    (borrowed\pared down from Corrin's ProfileViewModel!)
        // Below here is for Care Provider drop-down list 
        //      -- choose a Care Provider to care for your Pet
        //   ALSO -- to display Pet Carer's names in Appointment Listing Views
        public class CareProviderInfo
        {
            public int CareProviderID { get; set; }

            public string CP_FirstName { get; set; }
            public string CP_LastName { get; set; }
            public string CP_Name { get; set; }
            public string CP_Zipcode { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<CareProviderInfo> PetCarerList { get; set; }

        //===============================================================================
        //===============================================================================
        //                                                      CareTransactions table
        //===============================================================================
        // now the stuff from the actual CareTransactions table!
        public int TransactionID { get; set; }

        //===============================================================================
        [Column(TypeName = "date")]     // gets rid of '12:00:00 AM' appendage
        //
        // adding this to the data definition gives us a date-picker in the view
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        // it also formats things correctly for going into our db..... yay!
        //
        [DisplayName("Start Date:")]
        public DateTime StartDate { get; set; }


        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        [DisplayName("End Date:")]
        public DateTime EndDate { get; set; }

        //-------------------------------------------------------------------------------
        [Column(TypeName ="time")]                                    // CareTransactions
        [DataType(DataType.Time)]
        //[DisplayFormat(DataFormatString = "{0:hh\\:mm tt}", ApplyFormatInEditMode = true)]
        [DisplayName("Start Time:")]
        public TimeSpan StartTime { get; set; }

        //SERIOUSLY! DISPLAYING TIME IN 12-HOUR FORMAT SHOULsDN'T BE THIS F'ING DIFFICULT!
        // YES I TRIED CHANGING FROM 'TimeSpan' to 'DateTime'
        // I remember this shit being unconscionably difficult in 460 as well.

        [Column(TypeName = "time")]
        [DataType(DataType.Time)]
        //[DisplayFormat(DataFormatString = "{0:hh\\:mm tt}", ApplyFormatInEditMode = true)]
        [DisplayName("End Time:")]
        public TimeSpan EndTime { get; set; }

        //===============================================================================
        //                                                            // CareTransactions
        [DisplayName("Pet Care instructions this visit:")]
        public string NeededThisVisit { get; set; }

        [DisplayName("Pet Care recap:")]
        public string CareProvided { get; set; }

        [DisplayName("Full Pet Care Report -- Details:")]
        public string CareReport { get; set; }

        //-------------------------------------------------------------------------------
        //                                                            // CareTransactions
        [DisplayName("Pet Carer's fee for this visit:")]
        public float? Charge { get; set; }

        [DisplayName("Tip? (optional)")]
        public float? Tip { get; set; }

        //===============================================================================
        //[Range(0,5)] <= handle in .cshtml or else it can't be nullable!
        [DisplayName("Pet Carer Rating:")]
        public int? PC_Rating { get; set; }

        [DisplayName("Pet Carer Comments:  (from Pet Owner!)")]
        public string PC_Comments { get; set; }

        //-------------------------------------------------------------------------------
        //[Range(0,5)] <= handle in .cshtml or else it can't be nullable!
        [DisplayName("Pet Owner Rating:")]
        public int? PO_Rating { get; set; }

        [DisplayName("Pet Owner Comments:  (from Pet Carer!)")]
        public string PO_Comments { get; set; }

        //===============================================================================
        // FOREIGN KEYS FROM CareTransactions TABLE                   // CareTransactions
        
        [DisplayName("Pet's Owner:")]
        public int CT_PetOwnerID { get; set; }

        [DisplayName("Which Pet Carer?")]
        public int CT_CareProviderID { get; set; }

        [DisplayName("Which Pet?")]
        public int CT_PetID { get; set; }

        //===============================================================================
        //===============================================================================
        public class ApptInfo
        {
            [DisplayName("PetID:")]
            public int PetID { get; set; }

            [DisplayName("Pet's name:")]
            public string PetName { get; set; }
            //---------------------------------------------------------

            [DisplayName("PetOwnerID:")]
            public int PetOwnerID { get; set; }

            [DisplayName("Pet's Owner:")]
            public string PetOwnerName { get; set; }
            public string PetOwnerFirstName { get; set; }
            public string PetOwnerLastName { get; set; }

            //---------------------------------------------------------
            [DisplayName("PetCarerID:")]
            public int PetCarerID { get; set; }

            [DisplayName("Pet Carer:")]
            public string PetCarerName { get; set; }
            public string PetCarerFirstName { get; set; }
            public string PetCarerLastName { get; set; }

            //---------------------------------------------------------
            [Column(TypeName = "date")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
            public DateTime StartDate { get; set; }

            [Column(TypeName = "date")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
            public DateTime EndDate { get; set; }

            //---------------------------------------------------------
            [Column(TypeName = "time")]
            [DataType(DataType.Time)]
            public TimeSpan StartTime { get; set; }

            [Column(TypeName = "time")]
            [DataType(DataType.Time)]
            public TimeSpan EndTime { get; set; }

            //---------------------------------------------------------
            [DisplayName("Pet Care instructions this visit:")]
            public string NeededThisVisit { get; set; }

            [DisplayName("Pet Care recap:")]
            public string CareProvided { get; set; }

            [DisplayName("Full Pet Care Report -- Details:")]
            public string CareReport { get; set; }

            //---------------------------------------------------------
            [DisplayName("Pet Carer's fee for this visit:")]
            public float? Charge { get; set; }

            [DisplayName("Tip? (optional)")]
            public float? Tip { get; set; }

            //---------------------------------------------------------
            [DisplayName("Pet Carer Rating:")]
            public int? PC_Rating { get; set; }

            [DisplayName("Pet Carer Comments:  (from Pet Owner!)")]
            public string PC_Comments { get; set; }

            //---------------------------------------------------------
            [DisplayName("Pet Owner Rating:")]
            public int? PO_Rating { get; set; }

            [DisplayName("Pet Owner Comments:  (from Pet Carer!)")]
            public string PO_Comments { get; set; }

            //---------------------------------------------------------
            public int CareTransactionID { get; set; }
        }
        public List<ApptInfo> ApptInfoList { get; set; }

        //===============================================================================
        //===============================================================================
        // for the admin-level "every appointment" view
        public class IndexInfo
        {
            public string PetName { get; set; }
            public string PetOwnerFirstName { get; set; }
            public string PetOwnerLastName { get; set; }
            public string PetProviderFirstName { get; set; }
            public string PetProviderLastName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int TransactionID { get; set; }
        }
        public List<IndexInfo> IndexInfoList { get; set; }
    }
}