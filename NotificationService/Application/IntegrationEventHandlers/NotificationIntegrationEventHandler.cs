using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Hubs;
using NotificationService.Application.Types;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Application.IntegrationEventHandlers
{
    public class NotificationIntegrationEventHandler : IntegrationEventHandler<NotificationIntegrationEvent>
    {
        private readonly ILogger<NotificationIntegrationEventHandler> _logger;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IConnectionManager _connectionManager;

        public NotificationIntegrationEventHandler(ILogger<NotificationIntegrationEventHandler> logger,
            IHubContext<NotificationHub> notificationHubContext, IConnectionManager connectionManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationHubContext = notificationHubContext ?? throw new ArgumentNullException(nameof(notificationHubContext));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public override async Task Handle(NotificationIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(NotificationIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id));

            if (@event.ExpiresOn < DateTime.UtcNow) return;

            var notification = new Notification
            {
                Id = @event.Id,
                UserId = @event.UserId,
                CreatedAt = @event.CreatedAt,
                ExpiresOn = @event.ExpiresOn,
                Message = @event.Message,
                Details = @event.Details
            };

            var connectionIds = _connectionManager.Get(@event.UserId).ToList();
            if (connectionIds.Count == 0) return;

            const string methodName = "RECEIVE_NOTIF";
            foreach (var connectionId in connectionIds)
            {
                await _notificationHubContext.Clients.Client(connectionId).SendAsync(methodName);
            }

            _logger.Log(LogLevel.Information, "Notification pushed",
                "EventId".ToKvp(@event.Id),
                "UserId".ToKvp(@event.UserId));
        }
    }
}
