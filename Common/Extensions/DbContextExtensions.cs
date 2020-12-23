using Common.Types.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task DispatchDomainEvents(this DbContext context, IMediator mediator,
            CancellationToken cancellationToken = default)
        {
            var changedEntities = context.ChangeTracker
                .Entries<EntityBase>()
                .Where(x => x.Entity.DomainEvents?.Any() ?? false)
                .Select(x => x.Entity)
                .ToList();

            var domainEvents = changedEntities.SelectMany(x => x.DomainEvents);
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }

            foreach (var entity in changedEntities)
            {
                entity.ClearDomainEvents();
            }
        }
    }
}
