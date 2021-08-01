using Common.Domain.DomainEvents;
using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Common.Utilities.Types;
using Microsoft.Extensions.Logging;
using Orders.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.DomainEventHandlers
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
