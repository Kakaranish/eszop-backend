using Common.Authentication;
using Common.Extensions;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Types;
using System;
using System.Threading.Tasks;

namespace NotificationService.Hubs
{
    [JwtAuthorize]
    public class NotificationHub : Hub
    {
        private readonly IConnectionManager _connectionManager;

        public NotificationHub(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            var connectionId = Context.ConnectionId;

            _connectionManager.Add(userId, connectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            var connectionId = Context.ConnectionId;

            _connectionManager.Remove(userId, connectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
