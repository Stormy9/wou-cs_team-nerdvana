using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class ProfileViewModel
    {
        //everything from PetopiaUser
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsOwner { get; set; } //Need this to know if we should show owner info on page
        public bool IsProvider { get; set; } //Need this to know if we should show provider info on page
        public string ResState { get; set; }
        public byte[] ProfilePhoto { get; set; } //needSET
        //Average rating named changed because it shares name with other average rating
        //Everything from CareProvider
        public string ProviderAverageRating { get; set; }
        public string ExperienceDetails { get; set; }
        //Everything from PetOwner
        public string OwnerAverageRating { get; set; }
        public string NeedsDetails { get; set; }
        public string AccessInstructions { get; set; }
        //Everything below here won't be included in the profile page but can still be edited
        public string MainPhone { get; set; }
        public string AltPhone { get; set; }
        public string ResAddress01 { get; set; }
        public string ResAddress02 { get; set; }
        public string ResCity { get; set; }
        public string ResZipcode { get; set; }
    }
}