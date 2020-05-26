using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petopia.Models.ViewModels
{
    public class CareTransactionViewModel
    {
        //===============================================================================
        //===============================================================================
        //                                                        from PetopiaUsers table
        //===============================================================================
        // MAIN stuff from PetopiaUsers table:
        public int UserId { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("First name:")]                                      // PetopiaUsers
        public string FirstName { get; set; }

        [DisplayName("Last name:")]
        public string LastName { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Pet Owner?")]                                       // PetopiaUsers
        public bool IsOwner { get; set; }

        [DisplayName("Pet Carer?")]
        public bool IsProvider { get; set; }

        //===============================================================================
        // Background\Private Account Info from                        PetopiaUsers table
        [DisplayName("Main Phone #:")]
        public string MainPhone { get; set; }

        [DisplayName("Alternate Phone #:")]
        public string AltPhone { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Residential Street Address:")]                      // PetopiaUsers
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
        //                                                                     Pets table
        //===============================================================================
        // main things from Pet table:
        public int PetID { get; set; }

        [DisplayName("Pet's name:")]
        public string PetName { get; set; }

        //===============================================================================
        // the public/profile details from                                     Pet table:
        [DisplayName("Pet's species:")]
        public string Species { get; set; }

        [DisplayName("Pet's breed:")]
        public string Breed { get; set; }

        [DisplayName("Pet's gender:")]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Pet's Birthday:")]
        public DateTime Birthday { get; set; }

        //-------------------------------------------------------------------------------
        // the background\private details from                                 Pet table:
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
        // we will need a Pet List to make check-boxes for a User's Pets            Pets
        //   -- for owner to choose which Pet(s) they're booking care for           List
        //    ALSO -- to display Pet's names in Appointment Listing Views?
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
        //                                                                PetOwner Table
        //===============================================================================
        // 
        public int PetOwnerID { get; set; }

        // is there anything we can do with this?
        [DisplayName("Pet's Owner:")]
        public string PetOwnerName { get; set; }

        [DisplayName("My General Pet Care Needs:")]
        public string GeneralNeeds { get; set; }

        //-------------------------------------------------------------------------------
        // the "background\private info" from PetOwner table:
        [DisplayName("How to access our Home:")]
        public string HomeAccess { get; set; }

        // foreign key???
        public int PO_UserID { get; set; }

        //===============================================================================
        //===============================================================================
        //                                                          CareProvider table
        //===============================================================================
        // the "main info" from the CareProvider table:

        [DisplayName("Which Pet Carer?")]
        public int CareProviderID { get; set; }

        // is there anything we can do with this?
        [DisplayName("Pet Carer:")]
        public string PetCarerName { get; set; }

        // don't think we need any for this part of things?     well, maybe, later?
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

            public string CP_Name { get; set; }
            public string CP_Zipcode { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<CareProviderInfo> PetCarerList { get; set; }

        // was trying to set a SelectList here like some examples i saw online,
        //  but it just didn't like that -- now i forget why, haha.....
        //   i looked at and tried *SO* many examples!!!   [=
        //    okay just found stuff that says specifically to NOT do SelectLists here
        public List<CareProviderInfo> PetCarerSelectList { get; set; }


        //===============================================================================
        //===============================================================================
        //                                                         CareTransactions table
        //===============================================================================
        // now the stuff from the actual CareTransactions table!
        public int TransactionID { get; set; }

        //===============================================================================
        [Required(ErrorMessage = "please enter a start date")]
        [Column(TypeName = "date")]     // gets rid of '12:00:00 AM' appendage
        //
        // adding this to the data definition gives us a date-picker in the view
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        // it also formats things correctly for going into our db..... yay!
        //
        [DisplayName("Start Date:")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "please enter an end date")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        [DisplayName("End Date:")]
        public DateTime EndDate { get; set; }

        //-------------------------------------------------------------------------------
        [Required(ErrorMessage = "please enter a start time")]
        [DisplayName("Start Time:")]
        [DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "please enter an end time")]
        [DisplayName("End Time:")]
        [DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        //===============================================================================
        //                                                            // CareTransactions
        [Required(ErrorMessage = "please give instructions for your pet carer")]
        [DisplayName("Pet Care instructions for this visit:")]
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

        [DisplayName("Pet Carer Comments:")]
        public string PC_Comments { get; set; }

        //-------------------------------------------------------------------------------
        //[Range(0,5)] <= handle in .cshtml or else it can't be nullable!
        [DisplayName("Pet Owner Rating:")]
        public int? PO_Rating { get; set; }

        [DisplayName("Pet Owner Comments:")]
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
        // booleans for status of appointments                      // Care Transactions
        [DisplayName("Pending")]
        public bool Pending { get; set; }

        [DisplayName("Confirmed")]
        public bool Confirmed { get; set; }

        [DisplayName("Completed - Pet Owner")]
        public bool Completed_PO { get; set; }

        [DisplayName("Completed - Pet Carer")]
        public bool Completed_CP { get; set; }

        //===============================================================================
        //===============================================================================
        //                              for 'MyAppointments' + 'MyPetsAppointments' views
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

            public int PetOwnerPetopiaID { get; set; }

            [DisplayName("Pet's Owner:")]
            public string PetOwnerName { get; set; }

            public bool isPetOwner { get; set; }
            //---------------------------------------------------------
            [DisplayName("PetCarerID:")]
            public int PetCarerID { get; set; }

            public int PetCarerPetopiaID { get; set; }

            [DisplayName("Pet Carer:")]
            public string PetCarerName { get; set; }

            public bool isPetCarer { get; set; }
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
            [DisplayName("Start Time:")]
            [DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime StartTime { get; set; }

            [DisplayName("End Time:")]
            [DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Time)]
            public DateTime EndTime { get; set; }

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

            [DisplayName("Pet Carer Comments:")]
            public string PC_Comments { get; set; }

            //---------------------------------------------------------
            [DisplayName("Pet Owner Rating:")]
            public int? PO_Rating { get; set; }

            [DisplayName("Pet Owner Comments:")]
            public string PO_Comments { get; set; }

            //---------------------------------------------------------
            // ONLY FOR **CONFIRMED** APPOINTMENTS!
            public string PetOwner_Email { get; set; }
            public string PetOwner_MainPhone { get; set; }

            public string PetCarer_Email { get; set; }
            public string PetCarer_MainPhone { get; set; }

            //---------------------------------------------------------
            // booleans for status of appointments                      
            [DisplayName("Pending")]
            public bool Pending { get; set; }

            [DisplayName("Confirmed")]
            public bool Confirmed { get; set; }

            [DisplayName("Completed - Pet Owner")]
            public bool Completed_PO { get; set; }

            [DisplayName("Completed - Pet Carer")]
            public bool Completed_CP { get; set; }

            //---------------------------------------------------------
            public int CareTransactionID { get; set; }
        }
        public List<ApptInfo> ApptInfoListPending { get; set; }

        public List<ApptInfo> ApptInfoListConfirmed { get; set; }

        public List<ApptInfo> ApptInfoListUpcoming { get; set; }

        public List<ApptInfo> ApptInfoListFinished { get; set; }

        //===============================================================================
        //===============================================================================
        //                                  for the admin-level "every appointment" view
        //===============================================================================
        public class IndexInfo
        {
            public int PetID { get; set; }
            public string PetName { get; set; }
            public int PetOwnerID { get; set; }
            public string PetOwnerName { get; set; }
            public int PetProviderID { get; set; }
            public string PetProviderName { get; set; }


            [Column(TypeName = "date")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
            public DateTime StartDate { get; set; }

            [Column(TypeName = "date")]
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
            public DateTime EndDate { get; set; }
            public int TransactionID { get; set; }
        }
        public List<IndexInfo> IndexInfoList { get; set; }

        //===============================================================================
        //===============================================================================
    }
}