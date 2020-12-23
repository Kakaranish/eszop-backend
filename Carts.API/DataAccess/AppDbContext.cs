﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Carts.API.Domain;
using Common.Extensions;
using Common.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Carts.API.DataAccess
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        
        public DbSet<Cart> Carts{ get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        
        public AppDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

        public async Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            await this.DispatchDomainEvents(_mediator, cancellationToken);
            return await base.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
