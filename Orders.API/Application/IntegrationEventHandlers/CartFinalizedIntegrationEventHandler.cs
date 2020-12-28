using Common.IntegrationEvents;
using Common.ServiceBus;
using System.Threading.Tasks;

namespace Orders.API.Application.IntegrationEventHandlers
{
    public class CartFinalizedIntegrationEventHandler : IntegrationEventHandler<CartFinalizedIntegrationEvent>
    {
        public override Task Handle(CartFinalizedIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
