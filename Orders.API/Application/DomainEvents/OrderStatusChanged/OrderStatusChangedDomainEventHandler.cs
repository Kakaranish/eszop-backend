using Common.Domain.DomainEvents;
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

namespace Orders.API.Application.DomainEvents.OrderStatusChanged
{
    public class OrderStatusChangedDomainEventHandler : IDomainEventHandler<OrderStatusChangedDomainEvent>
    {
        private readonly ILogger<OrderStatusChangedDomainEventHandler> _logger;
        private readonly IEventBus _eventBus;

        public OrderStatusChangedDomainEventHandler(ILogger<OrderStatusChangedDomainEventHandler> logger,
            IEventBus eventBus)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task Handle(OrderStatusChangedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var notification = new NotificationIntegrationEvent
            {
                UserId = domainEvent.BuyerId,
                Code = NotificationCodes.OrderStateChanged,
                Message = "Order changed state",
                Metadata = new Dictionary<string, string>
                {
                    {"OrderId", domainEvent.OrderId.ToString()},
                    {"PreviousOrderState", domainEvent.PreviousState.Name},
                    {"CurrentOrderState", domainEvent.CurrentState.Name}
                }
            };

            await _eventBus.PublishAsync(notification);

            _logger.LogWithProps(LogLevel.Information,
                $"Published {nameof(NotificationIntegrationEvent)} after order status change",
                "EventId".ToKvp(notification.Id),
                "OrderId".ToKvp(domainEvent.OrderId));
        }
    }
}
