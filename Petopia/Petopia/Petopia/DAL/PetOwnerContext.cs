namespace Petopia.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PetOwnerContext : DbContext
    {
        public PetOwnerContext()
            : base("name=PetOwnerContext")
        {
        }

        public virtual DbSet<PetOwner> PetOwners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
