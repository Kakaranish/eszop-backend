﻿using Microsoft.EntityFrameworkCore;
using Offers.API.Domain;

namespace Offers.API.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Offer> Offers { get; private set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Offer>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,4)");
        }
    }
}