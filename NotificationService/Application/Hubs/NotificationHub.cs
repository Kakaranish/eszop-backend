using Common.Authentication;
using Common.Extensions;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NotificationService.Application.Commands.DeleteAll;
using NotificationService.Application.Commands.DeleteSingle;
using NotificationService.Application.Commands.ReadAll;
using NotificationService.DataAccess.Repositories;
using NotificationService.Extensions;

namespace NotificationService.Application.Hubs
{
    [JwtAuthorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        private readonly IConnectionManager _connectionManager;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMediator _mediator;
        private readonly INotificationCache _notificationCache;

        public NotificationHub(IConnectionManager connectionManager, 
            INotificationRepository notificationRepository, IMediator mediator, INotificationCache notificationCache)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _notificationCache = notificationCache ?? throw new ArgumentNullException(nameof(notificationCache));
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            var connectionId = Context.ConnectionId;

            _connectionManager.Add(userId, connectionId);

            var notifications = await _notificationCache.Get(userId);
            await Clients.Caller.SeedNotifications(notifications.Select(x => x.ToDto()));

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            var connectionId = Context.ConnectionId;

            _connectionManager.Remove(userId, connectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task ReadAllNotifications()
        {
            var readAllCommand = new ReadAllCommand();
            await _mediator.Send(readAllCommand);
        }

        public async Task DeleteAllNotifications()
        {
            var deleteAllCommand = new DeleteAllCommand();
            await _mediator.Send(deleteAllCommand);
        }

        public async Task DeleteNotification(string notificationId)
        {
            var deleteSingle = new DeleteSingleCommand { NotificationId = notificationId };
            await _mediator.Send(deleteSingle);
        }
    }
}
