using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Microsoft.Extensions.Logging;
using Offers.API.DataAccess.Repositories;
using System;
using System.Threading.Tasks;

namespace Offers.API.Application.IntegrationEventHandlers
{
    public class OrderCancelledIntegrationEventHandler : IntegrationEventHandler<OrderCancelledIntegrationEvent>
    {
        private readonly ILogger<OrderCancelledIntegrationEventHandler> _logger;
        private readonly IOfferRepository _offerRepository;

        public OrderCancelledIntegrationEventHandler(ILogger<OrderCancelledIntegrationEventHandler> logger,
            IOfferRepository offerRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public override async Task Handle(OrderCancelledIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OrderCancelledIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OrderId".ToKvp(@event.OrderId),
                "PreviousState".ToKvp(@event.PreviousState),
                "CurrentState".ToKvp(@event.CurrentState));

            foreach (var orderItem in @event.OrderItems)
            {
                var offer = await _offerRepository.GetByIdAsync(orderItem.OfferId);
                var currentAvailableStock = offer.AvailableStock;
                offer.SetAvailableStock(currentAvailableStock + orderItem.Quantity);

                _offerRepository.Update(offer);

                _logger.LogWithProps(LogLevel.Information, "Increased available stock",
                   "EventId".ToKvp(@event.Id),
                   "OfferId".ToKvp(orderItem.OfferId),
                   "QuantityToIncrease".ToKvp(orderItem.Quantity));
                _logger.LogInformation($"EventId={@event.Id}|OrderId={@event.OrderId}|OfferId={orderItem.OfferId}");
            }

            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            _logger.LogWithProps(LogLevel.Information,
                $"Handled {nameof(OrderCancelledIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OrderId".ToKvp(@event.OrderId));
        }
    }
}
