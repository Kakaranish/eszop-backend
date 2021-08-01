using Carts.Domain.Aggregates.CartAggregate;
using Carts.Domain.Aggregates.CartItemAggregate;
using Common.Domain.Repositories;
using Common.Utilities.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Carts.Infrastructure.DataAccess
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public DbSet<Cart> Carts { get; private set; }
        public DbSet<CartItem> CartItems { get; private set; }

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

            model.Entity<Cart>()
                .HasKey(x => x.Id);
        }

        public async Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            await this.DispatchDomainEvents(_mediator, cancellationToken);
            return await this.SaveChangesWithRetriesAsync(cancellationToken);
        }
    }
}
