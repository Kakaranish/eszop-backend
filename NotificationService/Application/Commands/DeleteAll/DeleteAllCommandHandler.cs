using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using NotificationService.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Application.Commands.DeleteAll
{
    public class DeleteAllCommandHandler : IRequestHandler<DeleteAllCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;

        public DeleteAllCommandHandler(IHttpContextAccessor httpContextAccessor,
            INotificationRepository notificationRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
        }

        public async Task<Unit> Handle(DeleteAllCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;

            _notificationRepository.RemoveAllByUserId(userId);
            await _notificationRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
