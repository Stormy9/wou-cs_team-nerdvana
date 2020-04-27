using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Petopia.Models
{
    [Table("PetGallery")]
    public partial class PetGallery
    {
        [Key]
        public int PetPicID { get; set; }
        [Required]
        public int? PetID { get; set; }
        [Required]
        public byte[] GalleryPhoto { get; set; }
        [StringLength(100)]
        public string Comment { get; set; }
        public virtual DAL.Pet Pet { get; set; }
    }
}