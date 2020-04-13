namespace Petopia.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CareTransaction")]
    public partial class CareTransaction
    {
        [Key]
        public int TransactionID { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [StringLength(90)]
        public string CareProvided { get; set; }

        public string CareReport { get; set; }

        [Column(TypeName = "money")]
        public decimal? Charge { get; set; }

        [Column(TypeName = "money")]
        public decimal? Tip { get; set; }

        public int? PC_Rating { get; set; }

        public string PC_Comments { get; set; }

        public int? PO_Rating { get; set; }

        public string PO_Comments { get; set; }

        public int PetOwnerID { get; set; }

        public int CareProviderID { get; set; }

        public int PetID { get; set; }
    }
}
