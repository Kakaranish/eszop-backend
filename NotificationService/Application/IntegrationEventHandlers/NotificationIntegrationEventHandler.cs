using Common.Utilities.EventBus;
using Common.Utilities.EventBus.IntegrationEvents;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.API.Application.Hubs;
using NotificationService.API.Application.Services;
using NotificationService.API.Extensions;
using NotificationService.Domain.Aggregates.NotificationAggregate;
using NotificationService.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.API.Application.IntegrationEventHandlers
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
            if (@event.Code != null) notification.SetCode(@event.Code);
            if (@event.Metadata != null) notification.SetMetadata(@event.Metadata);
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
