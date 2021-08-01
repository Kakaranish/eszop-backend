using Common.Domain.Repositories;
using NotificationService.Domain.Aggregates.NotificationAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Domain.Repositories
{
    public interface INotificationRepository : IDomainRepository<Notification>
    {
        Task<IList<Notification>> GetByUserId(Guid userId, int itemsNum);
        void Add(Notification notification);
        Task MarkAllAsRead(Guid userId);
        Task RemoveById(Guid notificationId);
        Task<IEnumerable<Guid>> RemoveAllExpired(TimeSpan expiration);
        void RemoveAllByUserId(Guid userId);
    }
}