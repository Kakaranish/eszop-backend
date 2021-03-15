using Common.Domain;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Common.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.DomainEvents.OrderConfirmed
{
    public class OrderConfirmedDomainEventHandler : IDomainEventHandler<OrderConfirmedDomainEvent>
    {
        private readonly ILogger<OrderConfirmedDomainEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public OrderConfirmedDomainEventHandler(ILogger<OrderConfirmedDomainEventHandler> logger,
            IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(OrderConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var notification = new NotificationIntegrationEvent
            {
                UserId = domainEvent.SellerId,
                Code = NotificationCodes.OrderConfirmed,
                Message = "Order has been confirmed",
                Metadata = new Dictionary<string, string>
                {
                    {"OrderId", domainEvent.OrderId.ToString()},
                    {"BuyerId", domainEvent.BuyerId.ToString()}
                }
            };

            await _eventBus.PublishAsync(notification);

            _logger.LogWithProps(LogLevel.Information,
                $"Published {nameof(NotificationIntegrationEvent)} after order has been confirmed",
                "EventId".ToKvp(notification.Id),
                "OrderId".ToKvp(domainEvent.OrderId));
        }
    }
}
