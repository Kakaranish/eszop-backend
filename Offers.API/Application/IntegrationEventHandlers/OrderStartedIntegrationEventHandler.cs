using Common.EventBus;
using Common.IntegrationEvents;
using Common.Types;
using Offers.API.DataAccess.Repositories;
using System;
using System.Threading.Tasks;

namespace Offers.API.Application.IntegrationEventHandlers
{
    public class OrderStartedIntegrationEventHandler : IntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IOfferRepository _offerRepository;

        public OrderStartedIntegrationEventHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public override async Task Handle(OrderStartedIntegrationEvent @event)
        {
            var offer = await _offerRepository.GetByIdAsync(@event.OfferId);
            if (offer is null)
            {
                throw new IntegrationEventException($"{@event.OfferId} offer does not exist");
            }
            if (offer.AvailableStock < @event.Quantity)
            {
                throw new IntegrationEventException($"AvailableStock is less than Quantity to decrease for offer {@event.OfferId}");
            }

            offer.DecreaseAvailableStock(@event.Quantity);
            _offerRepository.Update(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();
        }
    }
}
