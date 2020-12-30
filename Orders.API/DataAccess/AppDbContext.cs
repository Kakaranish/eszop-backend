﻿using Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common.DataAccess;

namespace Orders.API.DataAccess
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<OrderItem>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<OrderItem>()
                .Property(x => x.PricePerItem)
                .HasColumnType("decimal(18,4)");
        }

        public async Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            await this.DispatchDomainEvents(_mediator, cancellationToken);
            return await base.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
