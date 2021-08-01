using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Microsoft.Extensions.Logging;
using Offers.Domain.Repositories;
using Offers.Domain.Repositories.Types;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Offers.API.Application.IntegrationEventHandlers
{
    public class UserLockedIntegrationEventHandler : IntegrationEventHandler<UserLockedIntegrationEvent>
    {
        private readonly ILogger<UserLockedIntegrationEventHandler> _logger;
        private readonly IOfferRepository _offerRepository;

        public UserLockedIntegrationEventHandler(ILogger<UserLockedIntegrationEventHandler> logger,
            IOfferRepository offerRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
        }

        public override async Task Handle(UserLockedIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(UserLockedIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id),
                "UserId".ToKvp(@event.UserId));

            var offersPagination = await _offerRepository.GetAllActiveByUserIdAsync(@event.UserId,
                    new OfferFilter { PageSize = Int32.MaxValue });

            foreach (var offer in offersPagination.Items)
            {
                offer.EndOffer();
                _offerRepository.Update(offer);
            }

            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            _logger.LogWithProps(LogLevel.Information,
                $"Ended {offersPagination.Items.Count} offers",
                "EventId".ToKvp(@event.Id),
                "UserId".ToKvp(@event.UserId),
                "OfferIds".ToKvp(string.Join(",", offersPagination.Items.Select(x => x.Id))));
        }
    }
}
