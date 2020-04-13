namespace Petopia.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Petopia.Models;

    public partial class PetopiaContext : DbContext
    {
        //===============================================================================
        public PetopiaContext()
            : base("name=Petopia_Context_Azure")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        //===============================================================================
        public virtual DbSet<CareProvider> CareProviders { get; set; }
        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<PetopiaUser> PetopiaUsers { get; set; }
        public virtual DbSet<PetOwner> PetOwners { get; set; }
        public virtual DbSet<UserBadge> UserBadges { get; set; }
        public virtual DbSet<CareTransaction> CareTransactions { get; set; }

        //===============================================================================
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        //===============================================================================
    }
}
