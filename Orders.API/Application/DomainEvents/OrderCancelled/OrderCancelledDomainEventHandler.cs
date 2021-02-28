using Common.Domain;
using Common.Dto;
using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orders.API.Application.DomainEvents.OrderCancelled
{
    public class OrderCancelledDomainEventHandler : IDomainEventHandler<OrderCancelledDomainEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<OrderCancelledDomainEventHandler> _logger;

        public OrderCancelledDomainEventHandler(IEventBus eventBus, ILogger<OrderCancelledDomainEventHandler> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OrderCancelledDomainEvent)} domain event",
                "OrderId".ToKvp(domainEvent.OrderId));

            var integrationEvent = new OrderCancelledIntegrationEvent
            {
                OrderId = domainEvent.OrderId,
                OrderItems = domainEvent.OrderItems,
                PreviousState = Enum.Parse<OrderStateDto>(domainEvent.PreviousState.Name),
                CurrentState = Enum.Parse<OrderStateDto>(domainEvent.CurrentState.Name)
            };

            await _eventBus.PublishAsync(integrationEvent);

            _logger.LogWithProps(LogLevel.Information,
                $"Integration event {nameof(OrderCancelledIntegrationEvent)} published",
                "EventId".ToKvp(integrationEvent.Id),
                "PreviousState".ToKvp(integrationEvent.PreviousState.ToString()),
                "CurrentState".ToKvp(integrationEvent.CurrentState.ToString()),
                "OfferIds".ToKvp(string.Join(",", integrationEvent.OrderItems.Select(x => x.OfferId))));
        }
    }
}
