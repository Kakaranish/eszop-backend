﻿using Common.Domain.DomainEvents;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Identity.Domain.DomainEvents;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.DomainEventHandlers
{
    public class UserLockedDomainEventHandler : IDomainEventHandler<UserLockedDomainEvent>
    {
        private readonly ILogger<UserLockedDomainEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public UserLockedDomainEventHandler(ILogger<UserLockedDomainEventHandler> logger, IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(UserLockedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrationEvent = new UserLockedIntegrationEvent
            {
                UserId = domainEvent.UserId,
                LockedAt = domainEvent.LockedAt
            };

            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogWithProps(LogLevel.Information,
                $"Published {nameof(UserLockedIntegrationEvent)} integration event",
                "EventId".ToKvp(integrationEvent.Id),
                "UserId".ToKvp(domainEvent.UserId),
                "LockedAt".ToKvp(domainEvent.LockedAt));
        }
    }
}
