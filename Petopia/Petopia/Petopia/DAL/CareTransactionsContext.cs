namespace Petopia.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CareTransactionsContext : DbContext
    {
        public CareTransactionsContext()
            : base("name=CareTransactionsContext")
        {
        }

        public virtual DbSet<CareTransaction> CareTransactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
