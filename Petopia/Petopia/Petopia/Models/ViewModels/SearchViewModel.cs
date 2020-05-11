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
        //===============================================================================
        // care provider search: all the pieces we need -- ids, names, zips, pics, badges
        //===============================================================================
        public int CP_ID { get; set; }

        [DisplayName("CP_PU_ID")]
        public int CP_PU_ID { get; set; }

        public int PetopiaUserID { get; set; }

        [DisplayName("Name:")]
        public string CP_Name { get; set; }

        public string CP_FirstName { get; set; }

        public string CP_LastName { get; set; }

        // this is the same as for pet owner search
        [DisplayName("Zip:")]
        public string PU_ZipCode { get; set; }

        //-------------------------------------------------------------
        [DisplayName("user profile picture:")]
        public byte[] CP_Profile_Pic { get; set; } // this is the one

        public HttpPostedFileBase CP_Profile_Photo { get; set; }
        //-------------------------------------------------------------

        [DisplayName("My Pet Care Experience:")]
        public string ExperienceDetails { get; set; }

        [DisplayName("Average rating:")]
        public string ProviderAverageRating { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Dog?")]                              // care provider badges stuff
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
        //                                                     care provider search class
        //===============================================================================
        public class CareProviderSearch                     
        {
            [DisplayName("CP_ID")]
            public int CP_ID { get; set; }

            [DisplayName("CP_PU_ID")]
            public int CP_PU_ID { get; set; }

            [DisplayName("Name:")]
            public string CP_Name { get; set; }

            [DisplayName("PU_Zip:")]
            public string PU_Zipcode { get; set; }

            //---------------------------------------------------------
            [DisplayName("profile picture:")]
            public byte[] CP_Profile_Pic { get; set; } // this is the one

            public HttpPostedFileBase CP_Profile_Photo { get; set; }
            //---------------------------------------------------------

            [DisplayName("My Pet Care Experience:")]
            public string ExperienceDetails { get; set; }

            [DisplayName("Average rating:")]
            public string ProviderAverageRating { get; set; }

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
        }
        //-------------------------------------------------------------------------------
        // okay so i thought an `IQueryable` might be called for -- 
        //   however, for some reason, it wouldn't go `ToList()`.....
        //   it returned search results fine on the `if` -- but wouldn't do the `else`!
        //public IQueryable<CareProviderSearch> PetCarerSearchList { get; set; }

        // making this just a `List` like in the `CareTransactions` ViewModel.....
        //   this works, returns search results on the `if` & does the `else`, too
        public List<CareProviderSearch> PetCarerSearchList { get; set; }

        //===============================================================================
        //===============================================================================
        //     pet owner search: all the pieces we need -- ids, names, zips, pics, badges
        //===============================================================================
        public int PO_ID { get; set; }

        [DisplayName("PO_PU_ID")]
        public int PO_PU_ID { get; set; }

        //public int PetopiaUserID { get; set; }

        [DisplayName("Name:")]
        public string PO_Name { get; set; }

        public string PU_First_Name { get; set; }

        public string PU_Last_Name { get; set; }

        //[DisplayName("PU_Zip:")]
        //public string PU_Zip_Code { get; set; }

        //-------------------------------------------------------------
        [DisplayName("My profile picture:")]
        public byte[] PO_Profile_Pic { get; set; } // this is the one

        public HttpPostedFileBase PO_Profile_Photo { get; set; }
        //-------------------------------------------------------------

        [DisplayName("My Pet Care Needs:")]
        public string GeneralNeeds { get; set; }

        [DisplayName("Average rating:")]
        public string OwnerAverageRating { get; set; }

        //-------------------------------------------------------------------------------
        [DisplayName("Dog?")]                                   // pet owner bages stuff
        public bool IsDogOwner { get; set; }

        [DisplayName("Cat?")]
        public bool IsCatOwner { get; set; }

        [DisplayName("Bird?")]
        public bool IsBirdOwner { get; set; }

        [DisplayName("Fish?")]
        public bool IsFishOwner { get; set; }

        [DisplayName("Horse?")]
        public bool IsHorseOwner { get; set; }

        [DisplayName("Livestock?")]
        public bool IsLivestockOwner { get; set; }

        [DisplayName("Rabbit?")]
        public bool IsRabbitOwner { get; set; }

        [DisplayName("Reptile?")]
        public bool IsReptileOwner { get; set; }

        [DisplayName("Rodent?")]
        public bool IsRodentOwner { get; set; }

        [DisplayName("Other?")]
        public bool IsOtherOwner { get; set; }

        //===============================================================================
        //                                                        pet owner search class
        //===============================================================================
        public class PetOwnerSearch
        {
            [DisplayName("CP_ID")]
            public int PO_ID { get; set; }

            [DisplayName("CP_PU_ID")]
            public int PO_PU_ID { get; set; }

            [DisplayName("Name:")]
            public string PO_Name { get; set; }

            [DisplayName("PU_Zip:")]
            public string PU_Zipcode { get; set; }

            //---------------------------------------------------------
            [DisplayName("My profile picture:")]
            public byte[] PO_Profile_Pic { get; set; } // this is the one

            public HttpPostedFileBase PO_Profile_Photo { get; set; }
            //---------------------------------------------------------

            [DisplayName("My Pet Care Needs:")]
            public string GeneralNeeds { get; set; }

            [DisplayName("My average rating:")]
            public string OwnerAverageRating { get; set; }

            //---------------------------------------------------------
            //                                        add badge status!
            [DisplayName("Dog?")]
            public bool IsDogOwner { get; set; }

            [DisplayName("Cat?")]
            public bool IsCatOwner { get; set; }

            [DisplayName("Bird?")]
            public bool IsBirdOwner { get; set; }

            [DisplayName("Fish?")]
            public bool IsFishOwner { get; set; }

            [DisplayName("Horse?")]
            public bool IsHorseOwner { get; set; }

            [DisplayName("Livestock>?")]
            public bool IsLivestockOwner { get; set; }

            [DisplayName("Rabbit?")]
            public bool IsRabbitOwner { get; set; }

            [DisplayName("Reptile?")]
            public bool IsReptileOwner { get; set; }

            [DisplayName("Rodent?")]
            public bool IsRodentOwner { get; set; }

            [DisplayName("Other?")]
            public bool IsOtherOwner { get; set; }
        }
        //-------------------------------------------------------------------------------
        //                    see notes about this up in the Care Provivder Search Class
        // public IQueryable<PetOwnerSearch> PetOwnerSearchList { get; set; }

        public List<PetOwnerSearch> PetOwnerSearchList { get; set; }
        //===============================================================================
        //===============================================================================
    }
}