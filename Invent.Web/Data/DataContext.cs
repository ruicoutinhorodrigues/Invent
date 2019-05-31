using Invent.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Invent.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Inventory> Inventory { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<ProductModel> ProductModel { get; set; }

        public DbSet<Manufacturer> Manufacturer { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Size> Sizes { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Value)
                .HasColumnType("decimal(10,2)");

            var cascadeFKs = modelBuilder.Model
                           .GetEntityTypes()
                           .SelectMany(t => t.GetForeignKeys())
                           .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
