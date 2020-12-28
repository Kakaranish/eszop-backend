using System.Threading.Tasks;

namespace Common.EventBus
{
    public interface IEventBus
    {
        Task SubscribeAsync<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IntegrationEventHandler<TEvent>;

        Task PublishAsync<TEvent>(TEvent integrationEvent)
            where TEvent : IntegrationEvent;
    }
}
