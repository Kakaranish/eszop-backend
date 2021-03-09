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
        private readonly IConnectionManager _connectionManager;
        private readonly IHubContext<NotificationHub, INotificationClient> _notificationHubContext;

        public NotificationIntegrationEventHandler(ILogger<NotificationIntegrationEventHandler> logger,
            IConnectionManager connectionManager, IHubContext<NotificationHub, INotificationClient> notificationHubContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _notificationHubContext = notificationHubContext ?? throw new ArgumentNullException(nameof(notificationHubContext));
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

            await _notificationHubContext.Clients.Clients(connectionIds).ReceiveNotification(notification);

            _logger.LogWithProps(LogLevel.Information, "Notification pushed",
                "EventId".ToKvp(@event.Id),
                "UserId".ToKvp(@event.UserId));
        }
    }
}
