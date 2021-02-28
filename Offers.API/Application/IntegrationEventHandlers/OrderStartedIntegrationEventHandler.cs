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
    public class OrderStartedIntegrationEventHandler : IntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly ILogger<OrderStartedIntegrationEventHandler> _logger;

        public OrderStartedIntegrationEventHandler(IOfferRepository offerRepository,
            ILogger<OrderStartedIntegrationEventHandler> logger)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task Handle(OrderStartedIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(OrderStartedIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OrderId".ToKvp(@event.OrderId));

            foreach (var orderItem in @event.OrderItems)
            {
                var offer = await _offerRepository.GetByIdAsync(orderItem.OfferId);
                if (offer == null)
                    throw new IntegrationEventException($"Offer {orderItem.OfferId} not found");
                
                if (offer.AvailableStock < orderItem.Quantity)
                    throw new IntegrationEventException($"Illegal offer {orderItem.OfferId} quantity");
                var previousAvailableStock = offer.AvailableStock;
                offer.DecreaseAvailableStock(orderItem.Quantity);

                _logger.LogWithProps(LogLevel.Information,
                    "Decreased offer available stock",
                    "EventId".ToKvp(@event.Id),
                    "OfferId".ToKvp(offer.Id),
                    "PreviousValue".ToKvp(previousAvailableStock),
                    "CurrentValue".ToKvp(orderItem.Quantity));

                _offerRepository.Update(offer);
            }

            _logger.LogWithProps(LogLevel.Information,
                $"Handled {nameof(OrderStartedIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "OrderId".ToKvp(@event.OrderId));

            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();
        }
    }
}
