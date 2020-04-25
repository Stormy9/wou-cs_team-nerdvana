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
            //---------------------------------------------------------------------------
            // PetContext -- nothing in here in it's context

            //---------------------------------------------------------------------------
            // PetopiaUserContext -- nothing in here in it's context

            //---------------------------------------------------------------------------
            // PetOwnerContext -- nothing in here in it's context

            //---------------------------------------------------------------------------
        }
        //===============================================================================
    }
}
