using NotificationService.Domain.Aggregates.NotificationAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.API.Application.Services
{
    public interface INotificationCache
    {
        Task<IList<Notification>> Get(Guid userId);
        void Set(Guid userId, IEnumerable<Notification> notifications);
        void Delete(Guid userId);
        void PushIfExists(Guid userId, Notification notification);
    }
}
