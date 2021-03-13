using MediatR;
using NotificationService.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Application.Commands.DeleteSingle
{
    public class DeleteSingleCommandHandler : IRequestHandler<DeleteSingleCommand>
    {
        private readonly INotificationRepository _notificationRepository;

        public DeleteSingleCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
        }

        public async Task<Unit> Handle(DeleteSingleCommand request, CancellationToken cancellationToken)
        {
            var notificationId = Guid.Parse(request.NotificationId);

            _notificationRepository.RemoveById(notificationId);
            await _notificationRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
