﻿using Common.Domain.Repositories;
using Common.Utilities.Extensions;
using Identity.Domain.Aggregates.RefreshTokenAggregate;
using Identity.Domain.Aggregates.SellerInfoAggregate;
using Identity.Domain.Aggregates.UserAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Infrastructure.DataAccess
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;

        public DbSet<User> Users { get; private set; }
        public DbSet<RefreshToken> RefreshTokens { get; private set; }
        public DbSet<SellerInfo> SellerInfos { get; private set; }

        public AppDbContext(DbContextOptions options, IServiceProvider serviceProvider, IMediator mediator)
            : base(options)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
            modelBuilder.Entity<User>()
                .Property(x => x.DeliveryAddresses)
                .HasConversion(
                    x => x == null ? null : JsonConvert.SerializeObject(x),
                    x => x == null ? null : JsonConvert.DeserializeObject<List<DeliveryAddress>>(x));

            modelBuilder.Entity<RefreshToken>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<SellerInfo>()
                .HasKey(x => x.Id);

            //modelBuilder.Seed(_serviceProvider); // TODO
        }

        public async Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            await this.DispatchDomainEvents(_mediator, cancellationToken);
            return await this.SaveChangesWithRetriesAsync(cancellationToken);
        }
    }
}
