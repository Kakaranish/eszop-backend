using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.API.Application.Hubs;
using NotificationService.API.Application.Services;
using NotificationService.API.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.API.Application.Commands.RefreshCacheAndSeedUsers
{
    public class RefreshCacheAndSeedUsersCommandHandler : IRequestHandler<RefreshCacheAndSeedUsersCommand>
    {
        private readonly ILogger<RefreshCacheAndSeedUsersCommandHandler> _logger;
        private readonly INotificationCache _notificationCache;
        private readonly IConnectionManager _connectionManager;
        private readonly IHubContext<NotificationHub, INotificationClient> _notificationHubContext;

        public RefreshCacheAndSeedUsersCommandHandler(ILogger<RefreshCacheAndSeedUsersCommandHandler> logger,
            INotificationCache notificationCache, IConnectionManager connectionManager,
            IHubContext<NotificationHub, INotificationClient> notificationHubContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationCache = notificationCache ?? throw new ArgumentNullException(nameof(notificationCache));
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _notificationHubContext = notificationHubContext ?? throw new ArgumentNullException(nameof(notificationHubContext));
        }

        public async Task<Unit> Handle(RefreshCacheAndSeedUsersCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refreshing cache and seeding users");

            foreach (var userId in request.UserIds)
            {
                _notificationCache.Delete(userId);

                var connectionIds = _connectionManager.Get(userId).ToList();
                if (connectionIds.Count == 0) continue;

                var notifications = await _notificationCache.Get(userId);

                await _notificationHubContext.Clients.Clients(connectionIds)
                    .SeedNotifications(notifications.Select(x => x.ToDto()));
            }

            return await Unit.Task;
        }
    }
}
