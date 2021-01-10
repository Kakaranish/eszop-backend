﻿using Common.DataAccess;
using Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Offers.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.DataAccess
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public DbSet<Offer> Offers { get; private set; }
        public DbSet<Category> Categories { get; private set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; private set; }

        public AppDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Offer>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<DeliveryMethod>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<DeliveryMethod>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Category>()
                .HasKey(x => x.Id);
        }

        public async Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            await this.DispatchDomainEvents(_mediator, cancellationToken);
            return await base.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
