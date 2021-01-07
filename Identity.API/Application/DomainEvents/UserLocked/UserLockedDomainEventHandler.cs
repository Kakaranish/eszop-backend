using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.DomainEvents.UserLocked
{
    public class UserLockedDomainEventHandler : INotificationHandler<UserLockedDomainEvent>
    {
        public Task Handle(UserLockedDomainEvent notification, CancellationToken cancellationToken)
        {
            // TODO:
            return Task.CompletedTask;
        }
    }
}
