using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Petopia.Models.ViewModels
{
    public class SearchViewModel
    {
        [Key]
        public int CP_ID { get; set; }

        public int PetopiaUserID { get; set; }

        public string CP_FirstName { get; set; }

        public string CP_LastName { get; set; }

        public string PU_ZipCode { get; set; }

        //===============================================================================
        public class CareProviderSearch
        {
            [Key]
            public int CP_ID { get; set; }

            public string CP_FirstName { get; set; }
            public string CP_LastName { get; set; }
            public string CP_Name { get; set; }
            public string CP_Zipcode { get; set; }

            // add badge status!
            public bool IsDogProvider { get; set; }
            public bool IsCatProvider { get; set; }
            public bool IsBirdProvider { get; set; }
            public bool IsFishProvider { get; set; }
            public bool IsHorseProvider { get; set; }
            public bool IsLivestockProvider { get; set; }
            public bool IsRabbitProvider { get; set; }
            public bool IsReptileProvider { get; set; }
            public bool IsRodentProvider { get; set; }
            public bool IsOtherProvider { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<CareProviderSearch> PetCarerSearchList { get; set; }

        //===============================================================================
    }
}