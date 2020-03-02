namespace Petopia.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ProviderContext : DbContext
    {
        public ProviderContext()
            : base("name=ProviderContext")
        {
        }

        public virtual DbSet<Provider> Providers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
