using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.DomainEvents.UserUnlocked
{
    public class UserUnlockedDomainEventHandler : INotificationHandler<UserUnlockedDomainEvent>
    {
        public Task Handle(UserUnlockedDomainEvent notification, CancellationToken cancellationToken)
        {
            // TODO:
            return Task.CompletedTask;
        }
    }
}
