using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class ProfileViewModel
    {
        //===============================================================================
        //everything from PetopiaUser -- this is the profile-showing stuff...
        //                                the account-only stuff is down farther on here
        public int UserID { get; set; }

        //-------------------------------------------------------------
        [DisplayName("First name:")]
        public string FirstName { get; set; }

        [DisplayName("Last name:")]
        public string LastName { get; set; }

        //-------------------------------------------------------------------------------
        //Need this to know if we should show owner info on page          // PetopiaUsers
        public bool IsOwner { get; set; }

        //Need this to know if we should show provider info on page
        public bool IsProvider { get; set; }

        //-------------------------------------------------------------------------------
        // Profile Pic stuff!                                             // PetopiaUsers
        //
        [DisplayName("My profile picture:")]
        public byte[] ProfilePhoto { get; set; } //needSET

        public HttpPostedFileBase UserProfilePicture { get; set; }

        //-------------------------------------------------------------
        [DisplayName("Caption for my profile pic:")]
        public string UserCaption { get; set; }

        //-------------------------------------------------------------------------------
        // the rest of the profile-showing PetopiaUser stuff:             // PetopiaUsers
        //
        [DisplayName("General Location: (i.e., neighborhood name)")]
        public string GeneralLocation { get; set; }


        [DisplayName("My Bio: (some fun stuff about you)")]
        public string UserBio { get; set; }


        [DisplayName("Tagline to go under your name:")]
        public string Tagline { get; set; }

        //===============================================================================
        //Everything from CareProvider                                    // CareProvider
        //
        // profile thing:
        [DisplayName("My Pet Care Experience:")]
        public string ExperienceDetails { get; set; }

        // other thing:
        //AverageRating name changed because it shares name with other average rating
        [DisplayName("My average rating:")]
        public string ProviderAverageRating { get; set; }


        //===============================================================================
        //Everything from PetOwner                                          // PetOwner
        //
        // profile thing -- the thing we're after:
        [DisplayName("My general Pet Care needs:")]
        public string GeneralNeeds { get; set; }

        // account thing -- gotta be here to work right:
        [DisplayName("How to access our home:")]
        public string HomeAccess { get; set; }

        // other thing -- this too?
        [DisplayName("My average rating:")]
        public string OwnerAverageRating { get; set; }


        //===============================================================================
        //Everything below here won't be included in the profile page
        //   it's Account-Only, not profile-showing, stuff from            PetopiaUser...
        //
        //      it can be viewed FROM PetopiaUsers/MyAccountDetails
        //      and can still be edited of course -- FROM PetopiaUser/MyAccountEdit
        //
        // and it's gotta be here because it does -- to save things correctly.
        // and because it's part of creating a PetopiaUser record!   [=
        //
        [DisplayName("Main Phone #")]
        [StringLength(12)]
        public string MainPhone { get; set; }

        [DisplayName("Alternate Phone #")]
        [StringLength(12)]
        public string AltPhone { get; set; }

        [DisplayName("Residential Address:")]
        public string ResAddress01 { get; set; }

        [DisplayName("Address, 2nd line (if necessary):")]
        public string ResAddress02 { get; set; }

        [DisplayName("City:")]
        public string ResCity { get; set; }

        [DisplayName("State:")]
        [StringLength(2)]
        public string ResState { get; set; }

        [DisplayName("ZipCode:")]
        [StringLength(5)]
        public string ResZipcode { get; set; }

        //===============================================================================
        //Below here is for Pet Profiles  (for the Pet Cards on Owner Profiles)      Pets
        public class PetInfo
        {
            public int PetID { get; set; }
            public string PetName { get; set; }
            public string Species { get; set; }
            public string Breed { get; set; }
            public string Gender { get; set; }
            public byte[] PetPhoto { get; set; }
            public int? PetOwnersID { get; set; }

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
        //===============================================================================
        // i think we'll need this for the search result cards [in right-side column] ?
        //                        or something at least like this??
        public class petopiaUsersInfo
        { 
            public int UserID { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string GeneralLocation { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<petopiaUsersInfo> PetopiaUsersList { get; set; }

        //===============================================================================
        // i think we'll need this, too, for the search result cards in the profiles?
        //                               or something like it, for UserBadges??
        public class userBadgesInfo
        { 
            public int UserBadgeID { get; set; }

            public bool DogOwner { get; set; }

            public bool DogProvider { get; set; }

            public bool CatOwner { get; set; }

            public bool CatProvider { get; set; }

            public bool BirdOwner { get; set; }

            public bool BirdProvider { get; set; }

            // foreign key
            public int UserID { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<userBadgesInfo> UserBadgesList { get; set; }

        //===============================================================================
        //===============================================================================
    }
}