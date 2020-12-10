using Microsoft.EntityFrameworkCore;
using Products.API.Domain;

namespace Products.API.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; private set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Product>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,4)");
        }
    }
}
