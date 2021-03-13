using Common.DataAccess;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotificationService.Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.DataAccess.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly string _connectionString;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public NotificationRepository(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _connectionString = configuration.GetConnectionString("SqlServer");
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

        public void RemoveAllExpired(TimeSpan expiration)
        {
            var notificationsToRemove = _appDbContext.Notifications
                .Where(x => x.CreatedAt.AddMinutes(expiration.Minutes) < DateTime.UtcNow);

            _appDbContext.Notifications.RemoveRange(notificationsToRemove);
        }

        public async Task MarkAllAsRead(Guid userId)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = $"UPDATE Notifications SET {nameof(Notification.IsRead)}=@IsRead WHERE {nameof(Notification.UserId)}=@UserId";
            await connection.ExecuteAsync(query, new { UserId = userId, IsRead = true });
        }
    }
}
