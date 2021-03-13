using NotificationService.Application.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Application.Services
{
    public interface INotificationCache
    {
        Task<IList<Notification>> Get(Guid userId);
        void Set(Guid userId, IEnumerable<Notification> notifications);
        void PushIfExists(Guid userId, Notification notification);
    }
}
