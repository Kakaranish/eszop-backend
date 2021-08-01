namespace Common.Utilities.EventBus
{
    public class AzureEventBusConfig
    {
        public string ConnectionString { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }
    }
}
