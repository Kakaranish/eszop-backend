using Common.IntegrationEvents;
using Common.ServiceBus;
using System.Threading.Tasks;

namespace Carts.API.Application.IntegrationEventsHandlers
{
    public class OfferChangedEventHandler : IntegrationEventHandler<OfferChangedEvent>
    {
        public override Task Handle(OfferChangedEvent integrationEvent)
        {
            // TODO
            return Task.CompletedTask;
        }
    }
}
