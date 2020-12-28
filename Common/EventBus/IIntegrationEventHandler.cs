using System.Threading.Tasks;

namespace Common.EventBus
{
    public abstract class IntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
    {
        protected IntegrationEventHandler() { }

        public abstract Task Handle(TEvent @event);
    }
}
