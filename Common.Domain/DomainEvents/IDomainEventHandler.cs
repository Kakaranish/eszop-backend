using MediatR;

namespace Common.Domain.DomainEvents
{
    public interface IDomainEventHandler<in T> : INotificationHandler<T>
        where T : INotification
    {
    }
}
