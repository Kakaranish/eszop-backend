using Common.Domain;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.DomainEvents.UserLocked
{
    public class UserLockedDomainEventHandler : IDomainEventHandler<UserLockedDomainEvent>
    {
        private readonly IEventBus _eventBus;

        public UserLockedDomainEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(UserLockedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserLockedIntegrationEvent
            {
                UserId = domainEvent.UserId,
                LockedAt = domainEvent.LockedAt,
                LockedUntil = domainEvent.LockedUntil
            };

            await _eventBus.PublishAsync(integrationEvent);
        }
    }
}
