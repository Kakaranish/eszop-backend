using Common.Domain.Repositories;
using Common.Utilities.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotificationService.Domain.Aggregates.NotificationAggregate;
using NotificationService.Domain.Repositories;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.DataAccess.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public NotificationRepository(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IList<Notification>> GetByUserId(Guid userId, int itemsNum)
        {
            return await _appDbContext.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(itemsNum)
                .ToListAsync();
        }

        public void Add(Notification notification)
        {
            _appDbContext.Notifications.Add(notification);
        }

        public async Task MarkAllAsRead(Guid userId)
        {
            var sqlConnectionString = _configuration.GetSqlServerConnectionString();
            await using var connection = new SqlConnection(sqlConnectionString);
            await connection.OpenAsync();

            var query = $"UPDATE Notifications SET {nameof(Notification.IsRead)}=@IsRead WHERE {nameof(Notification.UserId)}=@UserId";

            var retryPolicy = Policy
                .Handle<DbUpdateException>()
                .Or<DbUpdateConcurrencyException>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

            await retryPolicy.ExecuteAsync(async ()
                => await connection.ExecuteAsync(query, new { UserId = userId, IsRead = true }));
        }

        public async Task RemoveById(Guid notificationId)
        {
            await _appDbContext.Notifications.Where(x => x.Id == notificationId).DeleteFromQueryAsync();
        }

        public async Task<IEnumerable<Guid>> RemoveAllExpired(TimeSpan expiration)
        {
            var notificationsToRemove = await _appDbContext.Notifications
                .Where(x => x.CreatedAt.AddMinutes(expiration.Minutes) < DateTime.UtcNow)
                .ToListAsync();

            var impactedUsers = notificationsToRemove.Select(x => x.UserId).Distinct();

            _appDbContext.Notifications.RemoveRange(notificationsToRemove);

            return impactedUsers;
        }

        public void RemoveAllByUserId(Guid userId)
        {
            _appDbContext.Notifications.Where(x => x.UserId == userId).DeleteFromQuery();
        }
    }
}
