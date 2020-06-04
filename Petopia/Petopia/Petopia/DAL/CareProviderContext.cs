namespace Petopia.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CareProviderContext : DbContext
    {
        public CareProviderContext()
            : base("name=CareProviderContext")
        {
        }

        //===============================================================================
        public virtual DbSet<CareProvider> CareProviders { get; set; }

        //-------------------------------------------------------------------------------
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        //===============================================================================
    }
}
