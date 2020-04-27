namespace Petopia.Models
{ 
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PetopiaUserContext : DbContext
    {
        public PetopiaUserContext()
            : base("name=PetopiaUserContext")
        {
        }

        //===============================================================================
        public virtual DbSet<PetopiaUser> PetopiaUsers { get; set; }

        //-------------------------------------------------------------------------------
        public virtual DbSet<PetOwner> PetOwners { get; set; }

        //-------------------------------------------------------------------------------
        public virtual DbSet<CareProvider> CareProviders { get; set; }

        //===============================================================================

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        //===============================================================================
    }
}
