namespace Petopia.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PetContext : DbContext
    {
        public PetContext()
            : base("name=PetContext")
        {
        }

        //===============================================================================
        public virtual DbSet<Pet> Pets { get; set; }

        //-------------------------------------------------------------------------------
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        //===============================================================================
    }
}
