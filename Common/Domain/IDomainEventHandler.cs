using MediatR;

namespace Common.Domain
{
    public interface IDomainEventHandler<in T> : INotificationHandler<T>
        where T : INotification
    {
    }
}
