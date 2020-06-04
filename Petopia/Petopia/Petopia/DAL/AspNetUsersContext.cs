namespace Petopia.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AspNetUsersContext : DbContext
    {
        public AspNetUsersContext()
            : base("name=AspNetUsersContext")
        {
        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
