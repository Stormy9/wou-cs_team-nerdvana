using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class RecViewModel
    {
        //===============================================================================
        //                                                     care provider search class
        //===============================================================================
        public class RecProviders
        {
            [DisplayName("CP_ID")]
            public int CP_ID { get; set; }

            [DisplayName("PO_ID")]
            public int PO_ID { get; set; }

            [DisplayName("UID")]
            public int UID { get; set; }

            [DisplayName("Name:")]
            public string Name { get; set; }

            [DisplayName("Zip:")]
            public string Zipcode { get; set; }

            public string GeneralLocation { get; set; }

            //---------------------------------------------------------

            [DisplayName("My Pet Care Experience:")]
            public string ExperienceDetails { get; set; }

            [DisplayName("Average rating:  ")]
            public decimal? ProviderAverageRating { get; set; }

            //---------------------------------------------------------
            //                                       add badge status!
            [DisplayName("Dog?")]
            public bool IsDogProvider { get; set; }

            [DisplayName("Cat?")]
            public bool IsCatProvider { get; set; }

            [DisplayName("Bird?")]
            public bool IsBirdProvider { get; set; }

            [DisplayName("Fish?")]
            public bool IsFishProvider { get; set; }

            [DisplayName("Horse?")]
            public bool IsHorseProvider { get; set; }

            [DisplayName("Livestock>?")]
            public bool IsLivestockProvider { get; set; }

            [DisplayName("Rabbit?")]
            public bool IsRabbitProvider { get; set; }

            [DisplayName("Reptile?")]
            public bool IsReptileProvider { get; set; }

            [DisplayName("Rodent?")]
            public bool IsRodentProvider { get; set; }

            [DisplayName("Other?")]
            public bool IsOtherProvider { get; set; }
            public decimal? Score { get; set; }
        }
        //-------------------------------------------------------------------------------

        public List<RecProviders> RecUserList { get; set; }


    }
}