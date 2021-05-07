namespace Kursovaya.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelDB : DbContext
    {
        public ModelDB()
            : base("name=ModelDB")
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Sdelka> Sdelka { get; set; }
        public virtual DbSet<Tovar> Tovar { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(e => e.Sdelka)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sdelka>()
                .Property(e => e.sum)
                .HasPrecision(15, 2);

            modelBuilder.Entity<Tovar>()
                .Property(e => e.price)
                .HasPrecision(15, 2);

            modelBuilder.Entity<Tovar>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Tovar>()
                .HasMany(e => e.Sdelka)
                .WithRequired(e => e.Tovar)
                .WillCascadeOnDelete(false);
        }
    }
}
