using System;
using Common.IntegrationEvents;
using Common.ServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class OfferChangedEventHandler : IntegrationEventHandler<OfferChangedEvent>
    {
        private readonly ILogger<OfferChangedEventHandler> _logger;

        public OfferChangedEventHandler(ILogger<OfferChangedEventHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Task Handle(OfferChangedEvent integrationEvent)
        {
            _logger.LogInformation($"Handling {nameof(OfferChangedEvent)} event for offer {integrationEvent.Id}");
            
            return Task.CompletedTask;
        }
    }
}
