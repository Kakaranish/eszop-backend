using Common.DataAccess;
using Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Orders.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.DataAccess
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public DbSet<Order> Orders { get; private set; }
        public DbSet<OrderItem> OrderItems { get; private set; }
        public DbSet<DeliveryAddress> DeliveryAddresses { get; private set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; private set; }

        public AppDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Order>()
                .Property(x => x.OrderState)
                .HasConversion(x => x.Name, x => OrderState.Parse(x))
                .HasDefaultValue(OrderState.Started);

            modelBuilder.Entity<OrderItem>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<OrderItem>()
                .Property(x => x.PricePerItem)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<DeliveryAddress>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<DeliveryMethod>()
                .HasKey(x => x.Id);
        }

        public async Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            await this.DispatchDomainEvents(_mediator, cancellationToken);
            return await this.SaveChangesWithRetriesAsync(cancellationToken);
        }
    }
}
