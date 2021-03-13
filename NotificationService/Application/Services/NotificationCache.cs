using Microsoft.Extensions.Caching.Memory;
using NotificationService.Application.Domain;
using NotificationService.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Application.Services
{
    public class NotificationCache : INotificationCache
    {
        public static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(60);

        private static readonly MemoryCacheEntryOptions CacheEntryOptions = new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.High)
            .SetSlidingExpiration(CacheExpiration);

        private readonly INotificationRepository _notificationRepository;
        private readonly MemoryCache _cache = new(new MemoryCacheOptions());

        public NotificationCache(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
        }

        public async Task<IList<Notification>> Get(Guid userId)
        {
            if (!_cache.TryGetValue(userId, out IList<Notification> notificationsResult))
            {
                const int notificationsNum = 50;
                var notifications = await _notificationRepository.GetByUserId(userId, notificationsNum);

                _cache.Set(userId, notifications, CacheEntryOptions);

                return notifications;
            }

            return notificationsResult;
        }

        public void Set(Guid userId, IEnumerable<Notification> notifications)
        {
            _cache.Set(userId, notifications, CacheEntryOptions);
        }

        public void Delete(Guid userId)
        {
            _cache.Remove(userId);
        }

        public void PushIfExists(Guid userId, Notification notification)
        {
            if (!_cache.TryGetValue(userId, out IList<Notification> notificationsResult)) return;

            notificationsResult.Add(notification);
            _cache.Set(userId, notificationsResult, CacheEntryOptions);
        }
    }
}
