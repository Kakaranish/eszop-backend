using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
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

        public ReadAllCommandHandler(INotificationRepository notificationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Unit> Handle(ReadAllCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            await _notificationRepository.MarkAllAsRead(userId);

            return await Unit.Task;
        }
    }
}
