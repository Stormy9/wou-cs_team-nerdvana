namespace Petopia.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OwnerContext : DbContext
    {
        public OwnerContext()
            : base("name=OwnerContext")
        {
        }

        public virtual DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
