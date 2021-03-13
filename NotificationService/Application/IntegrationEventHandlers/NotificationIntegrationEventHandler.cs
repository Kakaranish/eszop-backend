using Common.EventBus;
using Common.EventBus.IntegrationEvents;
using Common.Extensions;
using Common.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Domain;
using NotificationService.Application.Hubs;
using NotificationService.Application.Services;
using NotificationService.DataAccess.Repositories;
using NotificationService.Extensions;
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
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationCache _notificationCache;

        public NotificationIntegrationEventHandler(ILogger<NotificationIntegrationEventHandler> logger,
            IConnectionManager connectionManager, IHubContext<NotificationHub, INotificationClient> notificationHubContext,
            INotificationRepository notificationRepository, INotificationCache notificationCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _notificationHubContext = notificationHubContext ?? throw new ArgumentNullException(nameof(notificationHubContext));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _notificationCache = notificationCache ?? throw new ArgumentNullException(nameof(notificationCache));
        }

        public override async Task Handle(NotificationIntegrationEvent @event)
        {
            _logger.LogWithProps(LogLevel.Information,
                $"Handling {nameof(NotificationIntegrationEvent)} integration event",
                "EventId".ToKvp(@event.Id));

            var notification = new Notification(@event.UserId, @event.CreatedAt, @event.Message);
            if (@event.Details != null) notification.SetDetails(@event.Details);
            _notificationRepository.Add(notification);

            await _notificationRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync();

            _notificationCache.PushIfExists(@event.UserId, notification);

            var connectionIds = _connectionManager.Get(@event.UserId).ToList();
            if (connectionIds.Count == 0) return;

            await _notificationHubContext.Clients.Clients(connectionIds).ReceiveNotification(notification.ToDto());

            _logger.LogWithProps(LogLevel.Information, "Notification pushed",
                "EventId".ToKvp(@event.Id),
                "UserId".ToKvp(@event.UserId));
        }
    }
}
