using Common.DataAccess;
using NotificationService.Application.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.DataAccess.Repositories
{
    public interface INotificationRepository : IDomainRepository<Notification>
    {
        Task<IList<Notification>> GetByUserId(Guid userId, int itemsNum);
        void Add(Notification notification);
        void RemoveAllExpired(TimeSpan expiration);
        Task MarkAllAsRead(Guid userId);
    }
}
