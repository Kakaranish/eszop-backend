using Carts.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Carts.API.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cart> Carts{ get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<CartItem>()
                .HasKey(x => x.Id);
            model.Entity<CartItem>()
                .Property(x => x.PricePerItem)
                .HasColumnType("decimal(18,4)");
            model.Entity<CartItem>()
                .Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,4)");

            model.Entity<Domain.Cart>()
                .HasKey(x => x.Id);
        }
    }
}
