using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Petopia.Models.ViewModels
{
    public class PetGalleryViewModel
    {
        public int? CurrentPetID { get; set; }

        public HttpPostedFileBase GalleryPhoto { get; set; }

        [StringLength(100)]
        public string PhotoComment { get; set; }

        public class PetGalleryInfo
        {
            public int PetPicID { get; set; }
            public int? PetID { get; set; }
            public string Comment { get; set; }
        }
        public List<PetGalleryInfo> PetGalleryList { get; set; }
    }
}