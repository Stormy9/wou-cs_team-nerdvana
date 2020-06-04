using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class PetProfileViewModel
    {
        public DAL.Pet Pet { get; set; }

        public class PetopiaUsersInfo
        {
            public int UserID { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string GeneralLocation { get; set; }

            public byte[] ProfilePic { get; set; }

            public int UserBadgeID { get; set; }

            public bool DogProvider { get; set; }

            public bool CatProvider { get; set; }

            public bool BirdProvider { get; set; }

            public bool FishProvider { get; set; }

            public bool HorseProvider { get; set; }

            public bool LivestockProvider { get; set; }

            public bool RabbitProvider { get; set; }

            public bool ReptileProvider { get; set; }

            public bool RodentProvider { get; set; }

            public bool OtherProvider { get; set; }

            public bool DogOwner { get; set; }

            public bool CatOwner { get; set; }

            public bool BirdOwner { get; set; }

            public bool FishOwner { get; set; }

            public bool HorseOwner { get; set; }

            public bool LivestockOwner { get; set; }

            public bool RabbitOwner { get; set; }

            public bool ReptileOwner { get; set; }

            public bool RodentOwner { get; set; }

            public bool OtherOwner { get; set; }

            public decimal? ProviderAverageRating { get; set; }
            public decimal? Score { get; set; }
        }

        public List<PetopiaUsersInfo> PetopiaUsersList { get; set; }
    }
}