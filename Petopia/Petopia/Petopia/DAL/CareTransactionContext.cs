namespace Petopia.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CareTransactionContext : DbContext
    {
        public CareTransactionContext()
            : base("name=CareTransactionContext")
        {
        }

        public virtual DbSet<CareTransaction> CareTransactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CareTransaction>()
                .Property(e => e.Charge)
                .HasPrecision(19, 4);

            modelBuilder.Entity<CareTransaction>()
                .Property(e => e.Tip)
                .HasPrecision(19, 4);
        }
    }
}
