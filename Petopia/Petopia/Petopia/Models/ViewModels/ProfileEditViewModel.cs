using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class ProfileEditViewModel
    {
        public PetopiaUser PetUser { get; set; }
        public PetOwner OwnerUser { get; set; }
        public CareProvider ProviderUser { get; set; }
        public byte ProfilePic { get; set; }
    }
}