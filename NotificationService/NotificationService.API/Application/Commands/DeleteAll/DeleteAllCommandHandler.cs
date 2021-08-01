using Common.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NotificationService.API.Application.Services;
using NotificationService.Domain.Aggregates.NotificationAggregate;
using NotificationService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.API.Application.Commands.DeleteAll
{
    public class DeleteAllCommandHandler : IRequestHandler<DeleteAllCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationCache _notificationCache;

        public DeleteAllCommandHandler(IHttpContextAccessor httpContextAccessor,
            INotificationRepository notificationRepository, INotificationCache notificationCache)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _notificationCache = notificationCache ?? throw new ArgumentNullException(nameof(notificationCache));
        }

        public async Task<Unit> Handle(DeleteAllCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            _notificationRepository.RemoveAllByUserId(userId);
            await _notificationRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _notificationCache.Set(userId, new List<Notification>());

            return await Unit.Task;
        }
    }
}
