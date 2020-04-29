using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class PetGalleryViewModel
    {
        public int? CurrentPetID { get; set; }


        [DisplayName("Add new gallery picture for your Pet!")]
        public HttpPostedFileBase GalleryPhoto { get; set; }


        [StringLength(100)]
        [DisplayName("Add a caption for your picture:")]
        public string PhotoComment { get; set; }


        //===============================================================================
        public class PetGalleryInfo
        {
            public int PetPicID { get; set; }
            public int? PetID { get; set; }
            public string Comment { get; set; }
        }
        //-------------------------------------------------------------------------------
        public List<PetGalleryInfo> PetGalleryList { get; set; }
        
        //===============================================================================
    }
}