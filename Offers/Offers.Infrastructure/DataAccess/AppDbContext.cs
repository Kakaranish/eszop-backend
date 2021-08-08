using Common.Domain.EventDispatching;
using Common.Domain.Repositories;
using Common.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Offers.Domain.Aggregates.CategoryAggregate;
using Offers.Domain.Aggregates.OfferAggregate;
using Offers.Domain.Aggregates.PredefinedDeliveryMethodAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.Infrastructure.DataAccess
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IEventDispatcher _eventDispatcher;

        public DbSet<Offer> Offers { get; private set; }
        public DbSet<Category> Categories { get; private set; }
        public DbSet<PredefinedDeliveryMethod> PredefinedDeliveryMethods { get; private set; }

        public AppDbContext(DbContextOptions options, IEventDispatcher eventDispatcher) : base(options)
        {
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Offer>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Offer>()
                .Property(x => x.DeliveryMethods)
                .HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<List<DeliveryMethod>>(x));
            modelBuilder.Entity<Offer>()
                .Property(x => x.Images)
                .HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<List<ImageInfo>>(x));
            modelBuilder.Entity<Offer>()
                .Property(x => x.KeyValueInfos)
                .HasConversion(
                    x => x == null ? null : JsonConvert.SerializeObject(x),
                    x => x == null ? null : JsonConvert.DeserializeObject<List<KeyValueInfo>>(x));

            modelBuilder.Entity<Category>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PredefinedDeliveryMethod>()
                .HasKey(x => x.Id);
        }

        public async Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var changedEntities = this.GetChangedEntities();
            await _eventDispatcher.Dispatch(changedEntities);

            return await this.SaveChangesWithRetriesAsync(cancellationToken);
        }
    }
}
