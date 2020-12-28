using System.Threading.Tasks;

namespace Common.ServiceBus
{
    public abstract class IntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
    {
        protected IntegrationEventHandler() { }

        public abstract Task Handle(TEvent @event);
    }
}
