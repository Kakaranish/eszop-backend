using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NotificationService.Application.Services;
using NotificationService.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Application.Commands.ReadAll
{
    public class ReadAllCommandHandler : IRequestHandler<ReadAllCommand>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationCache _notificationCache;

        public ReadAllCommandHandler(INotificationRepository notificationRepository,
            IHttpContextAccessor httpContextAccessor, INotificationCache notificationCache)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _notificationCache = notificationCache ?? throw new ArgumentNullException(nameof(notificationCache));
        }

        public async Task<Unit> Handle(ReadAllCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            await _notificationRepository.MarkAllAsRead(userId);

            var notifications = await _notificationCache.Get(userId);
            foreach (var notification in notifications)
            {
                notification.SetIsRead(true);
            }
            _notificationCache.Set(userId, notifications);

            return await Unit.Task;
        }
    }
}
