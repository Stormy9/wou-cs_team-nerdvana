using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class MyAccountViewModel
    {
        //===============================================================================
        //     this is for getting the private account info of a user
        //     pet care providers have no "extra" private info like pet owners
        //        so they aren't on here   [=
        //===============================================================================
        // account things from PetopiaUser
        // 
        public int UserID { get; set; }

        //-------------------------------------------------------------
        [DisplayName("First Name:")]
        public string FirstName { get; set; }

        [DisplayName("Last Name:")]
        public string LastName { get; set; }

        //-------------------------------------------------------------

        [DisplayName("Main Phone:")]
        public string MainPhone { get; set; }

        [DisplayName("Alt Phone:")]
        public string AltPhone { get; set; }

        //-------------------------------------------------------------

        [DisplayName("Address:")]
        public string ResAddress01 { get; set; }

        [DisplayName("Address:")]
        public string ResAddress02 { get; set; }

        [DisplayName("City:")]
        public string ResCity { get; set; }

        [DisplayName("State:")]
        public string ResState { get; set; }

        [DisplayName("Zip:")]
        public string ResZipcode { get; set; }


        //===============================================================================
        //Need this to know if we should show owner info on page
        public bool IsOwner { get; set; } 


        //Need this to know if we should show provider info on page
        public bool IsProvider { get; set; }


        //===============================================================================
        // account thing from PetOwner:
        //
        [DisplayName("How To Access Our Home:")]
        public string HomeAccess { get; set; }

        //===============================================================================

    }
}