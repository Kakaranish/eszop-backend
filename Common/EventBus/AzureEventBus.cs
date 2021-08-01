using Common.Logging;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.EventBus
{
    public class AzureEventBus : IEventBus
    {
        private readonly ILogger<AzureEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SubscriptionClient _subscriptionClient;
        private readonly TopicClient _topicClient;
        private readonly ISet<string> _subscribedEvents = new HashSet<string>();

        public AzureEventBus(ILogger<AzureEventBus> logger, IOptions<AzureEventBusConfig> options, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            var eventBusConfig = options.Value ?? throw new ArgumentNullException();
            _topicClient = new TopicClient(eventBusConfig.ConnectionString, eventBusConfig.TopicName, RetryPolicy.Default);
            _subscriptionClient = new SubscriptionClient(eventBusConfig.ConnectionString, eventBusConfig.TopicName,
                eventBusConfig.SubscriptionName);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 5,
                AutoComplete = false
            };
            _subscriptionClient.RegisterMessageHandler(ProcessEvent, messageHandlerOptions);
        }

        public async Task SubscribeAsync<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IntegrationEventHandler<TEvent>
        {
            try
            {
                var eventTypeName = typeof(TEvent).Name;
                if (!_subscribedEvents.Add(eventTypeName))
                {
                    _logger.LogWarning($"Event {eventTypeName} is already subscribed");
                    return;
                }

                await _subscriptionClient.AddRuleAsync(new RuleDescription
                {
                    Filter = new CorrelationFilter { Label = eventTypeName },
                    Name = eventTypeName
                });
                _logger.LogInformation($"Created service bus subscription rule for {typeof(TEvent).Name}");
            }
            catch (ServiceBusException) { }
        }

        public async Task PublishAsync<TEvent>(TEvent integrationEvent) where TEvent : IntegrationEvent
        {
            if (_topicClient.IsClosedOrClosing)
            {
                _logger.LogError("Cannot publish event - service bus topic client is closed or closing");
                return;
            }

            var eventStr = JsonConvert.SerializeObject(integrationEvent);
            var eventBytes = Encoding.UTF8.GetBytes(eventStr);
            var message = new Message
            {
                Body = eventBytes,
                Label = typeof(TEvent).Name
            };

            await _topicClient.SendAsync(message);
        }

        private async Task ProcessEvent(Message message, CancellationToken cancellationToken)
        {
            var eventTypeName = message.Label;
            var eventType = typeof(AzureEventBus).Assembly.GetTypes().FirstOrDefault(x => x.Name == eventTypeName);

            var eventHandlerType = typeof(IntegrationEventHandler<>).MakeGenericType(eventType);
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService(eventHandlerType);

            if (!_subscribedEvents.Contains(eventTypeName) || handler == null)
            {
                _logger.LogWithProps(LogLevel.Critical,
                    $"Unable to process {eventTypeName} because there is no corresponding handler");
                return;
            }

            var integrationEventStr = Encoding.UTF8.GetString(message.Body);
            var integrationEvent = JsonConvert.DeserializeObject(integrationEventStr, eventType);

            const string methodName = nameof(IntegrationEventHandler<IntegrationEvent>.Handle);
            var method = eventHandlerType.GetMethod(methodName);
            await (Task)method.Invoke(handler, new[] { integrationEvent });

            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Error while handling integration event");

            return Task.CompletedTask;
        }
    }
}
