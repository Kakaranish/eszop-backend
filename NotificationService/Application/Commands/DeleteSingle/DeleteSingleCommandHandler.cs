using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NotificationService.Application.Services;
using NotificationService.DataAccess.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Application.Commands.DeleteSingle
{
    public class DeleteSingleCommandHandler : IRequestHandler<DeleteSingleCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationCache _notificationCache;

        public DeleteSingleCommandHandler(IHttpContextAccessor httpContextAccessor,
            INotificationRepository notificationRepository, INotificationCache notificationCache)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _notificationCache = notificationCache ?? throw new ArgumentNullException(nameof(notificationCache));
        }

        public async Task<Unit> Handle(DeleteSingleCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var notificationId = Guid.Parse(request.NotificationId);

            await _notificationRepository.RemoveById(notificationId);

            var notifications = await _notificationCache.Get(userId);
            _notificationCache.Set(userId, notifications.Where(x => x.Id != notificationId));

            return await Unit.Task;
        }
    }
}
