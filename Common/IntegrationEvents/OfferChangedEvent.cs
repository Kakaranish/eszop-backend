using Common.ServiceBus;

namespace Common.IntegrationEvents
{
    public class OfferChangedEvent : IntegrationEvent
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal? Price { get; private set; }

        public OfferChangedEvent(string name, string description, decimal? price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
