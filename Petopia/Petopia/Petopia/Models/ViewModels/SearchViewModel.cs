using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Petopia.DAL;
using Petopia.Models;

namespace Petopia.Models.ViewModels
{
    public class SearchViewModel
    {
        public int CP_ID { get; set; }

        [DisplayName("CP_PU_ID")]
        public int CP_PU_ID { get; set; }

        public int PetopiaUserID { get; set; }

        [DisplayName("Name:")]
        public string CP_Name { get; set; }

        public string CP_FirstName { get; set; }

        public string CP_LastName { get; set; }

        [DisplayName("CP_Zip:")]
        public string PU_ZipCode { get; set; }

        //-------------------------------------------------------------
        [DisplayName("My profile picture:")]
        public byte[] CP_Profile_Pic { get; set; } // this is the one

        public HttpPostedFileBase CP_Profile_Photo { get; set; }
        //-------------------------------------------------------------

        [DisplayName("My Pet Care Experience:")]
        public string ExperienceDetails { get; set; }

        [DisplayName("My average rating:")]
        public string ProviderAverageRating { get; set; }

        //-------------------------------------------------------------------------------
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

        [DisplayName("Livestock?")]
        public bool IsLivestockProvider { get; set; }

        [DisplayName("Rabbit?")]
        public bool IsRabbitProvider { get; set; }

        [DisplayName("Reptile?")]
        public bool IsReptileProvider { get; set; }

        [DisplayName("Rodent?")]
        public bool IsRodentProvider { get; set; }

        [DisplayName("Other?")]
        public bool IsOtherProvider { get; set; }

        //===============================================================================
        public class CareProviderSearch
        {
            [DisplayName("CP_ID")]
            public int CP_ID { get; set; }

            [DisplayName("CP_PU_ID")]
            public int CP_PU_ID { get; set; }

            [DisplayName("Name:")]
            public string CP_Name { get; set; }

            [DisplayName("CP_Zip")]
            public string CP_Zipcode { get; set; }

            //---------------------------------------------------------
            [DisplayName("My profile picture:")]
            public byte[] CP_Profile_Pic { get; set; } // this is the one

            public HttpPostedFileBase CP_Profile_Photo { get; set; }
            //---------------------------------------------------------

            [DisplayName("My Pet Care Experience:")]
            public string ExperienceDetails { get; set; }

            [DisplayName("My average rating:")]
            public string ProviderAverageRating { get; set; }

            //---------------------------------------------------------
            // add badge status!
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
        }
        //-------------------------------------------------------------------------------
        public IQueryable<CareProviderSearch> PetCarerSearchList { get; set; }

        //===============================================================================
    }
}