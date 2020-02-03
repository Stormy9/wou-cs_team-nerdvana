namespace swimApp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AthleteContext : DbContext
    {
        public AthleteContext()
            : base("name=AthleteContext")
        {
        }

        public virtual DbSet<Athlete> Athletes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
