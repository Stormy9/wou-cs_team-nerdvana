using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace Petopia.Models
{
    public class EmailModel
    {
        
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToName { get; set; }
        [Required, Display(Name = "Recipient email"), EmailAddress]
        public string ToEmail { get; set; }
        public string Message { get; set; }
    }
}