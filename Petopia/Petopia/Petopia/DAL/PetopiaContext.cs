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

        //-------------------------------------------------------------------------------
        public virtual DbSet<Pet> Pets { get; set; }

        //-------------------------------------------------------------------------------
        public virtual DbSet<PetopiaUser> PetopiaUsers { get; set; }

        //-------------------------------------------------------------------------------
        public virtual DbSet<PetOwner> PetOwners { get; set; }

        //------------------------------------------------------------------------------
        public virtual DbSet<UserBadge> UserBadges { get; set; }

        //-------------------------------------------------------------------------------
        public virtual DbSet<CareTransaction> CareTransactions { get; set; }

        //===============================================================================
        public virtual DbSet<PetGallery> PetGallery { get; set; }

        //===============================================================================
        public virtual DbSet<AspNetUser> ASPNetUsers { get; set; }

        //===============================================================================

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // CareProviderContext -- nothing in here in it's context

            //---------------------------------------------------------------------------
            // CareTransactionsContext -- 
            //
            // once i changed db data type from 'money' to 'float' -- for 'charge' & 'tip'
            // because 'money' data type is not nullable and we need this to be nullable!
            // i had to re-generate a context & model ..... 
            //      the new context has nothing here   [=
            //
            modelBuilder.Entity<CareTransaction>()
                .Property(e => e.Charge)
                .HasPrecision(5, 2);

            modelBuilder.Entity<CareTransaction>()
                .Property(e => e.Tip)
                .HasPrecision(5, 2);
            //---------------------------------------------------------------------------
            // PetContext -- nothing in here in it's context

            //---------------------------------------------------------------------------
            // PetopiaUserContext -- nothing in here in it's context

            //---------------------------------------------------------------------------
            // PetOwnerContext -- nothing in here in it's context

            //---------------------------------------------------------------------------
        }
        // tried scaffolding 'Views/Home/PetCarerSearchResult' 
        //    just to see what I'd get -- and it stuck this in here.....
        //
        //public System.Data.Entity.DbSet<Petopia.Models.ViewModels.SearchViewModel> SearchViewModels { get; set; }
        //===============================================================================
    }
}
