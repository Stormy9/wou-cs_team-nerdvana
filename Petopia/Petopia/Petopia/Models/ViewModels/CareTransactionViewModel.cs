using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petopia.Models.ViewModels
{
    public class CareTransactionViewModel
    {
        //===============================================================================
        //===============================================================================
        // MAIN stuff from PetOwner table:
        public int PetOwnerID { get; set; }

        // so..... what do we do about getting the FK `UserID` from PetOwner table?
        // and there's a FK `UserId` in the CareProvider table too.....

        //===============================================================================
        // MAIN stuff from CareProvider table:
        public int CareProviderID { get; set; }

        //===============================================================================
        //===============================================================================
        // MAIN stuff from PetopiaUsers table:
        public int PetOwnerUserId { get; set; }

        public int PetCarerUserId { get; set; }

        //-------------------------------------------------------------------------------
        public string FirstName { get; set; }

        public string LastName { get; set; }

        //-------------------------------------------------------------------------------
        public bool IsOwner { get; set; }
        public bool IsProvider { get; set; }

        //===============================================================================
        //===============================================================================
        // main stuff from Pet table:
        public int PetID { get; set; }


        public string PetName { get; set; }

        //===============================================================================
        //===============================================================================
        // the "background info" from PetopiaUsers table:
        public string MainPhone { get; set; }

        public string AltPhone { get; set; }

        public string ResAddress01 { get; set; }

        public string ResAddress02 { get; set; }

        public string ResCity { get; set; }

        public string ResState { get; set; }

        public string ResZipcode { get; set; }

        //===============================================================================
        // the "background info" from Pet table:
        public string Species { get; set; }

        public string Breed { get; set; }

        public string Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }

        public string Weight { get; set; }

        public string PetAccess { get; set; }

        public string NeedsDetails { get; set; }

        public string HealthConcerns { get; set; }

        public string BehaviorConcerns { get; set; }

        public string EmergencyContactName { get; set; }

        public string EmergencyContactPhone { get; set; }

        //-------------------------------------------------------------------------------
        // not sure if we need this here?  stuck it here for now though
        //    (borrowed from Corrin's ProfileViewModel!)
        // Below here is for Pet Profiles  (for the Pet Cards on Owner pages right?)
        public class PetInfo
        {
            public int PetID { get; set; }
            public string PetName { get; set; }
            public string Species { get; set; }
            public string Breed { get; set; }
            public string Gender { get; set; }

            [Column(TypeName = "date")]
            // that ^ is supposed to get rid of the time appendage on the Pet b-days on
            // the 'Pet Cards' on Owner profiles..... but, it isn't for some dumb reason
            [DataType(DataType.Date)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
            public DateTime Birthdate { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<PetInfo> PetList { get; set; }

        //===============================================================================
        // the "background info" from PetOwner table:
        public string HomeAccess { get; set; }

        public string GeneralNeeds { get; set; }

        //===============================================================================
        // the "background info" from the CareProvider table:

        // don't think we need any for this part of things?     well, maybe?
        public string ExperienceDetails { get; set; }

        //===============================================================================
        //===============================================================================
    }
}