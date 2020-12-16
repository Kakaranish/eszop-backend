using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>()
                .Property(x => x.HashedPassword)
                .HasConversion(x => x.ToString(), x => new HashedPassword(x));
            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasConversion(x => x.Name, x => Role.Parse(x));

            modelBuilder.Entity<RefreshToken>()
                .HasKey(x => x.Id);
        }
    }
}
