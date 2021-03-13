using CronScheduler.Extensions.Scheduler;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NotificationService.DataAccess.Repositories;

namespace NotificationService.Application
{
    public class CleanupJob : IScheduledJob
    {
        private readonly ILogger<CleanupJob> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationSettings _notificationSettings;
        public string Name => nameof(CleanupJob);

        public CleanupJob(ILogger<CleanupJob> logger, INotificationRepository notificationRepository, IOptions<NotificationSettings> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _notificationSettings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var expiration = TimeSpan.FromMinutes(_notificationSettings.ExpirationInMinutes);
            _notificationRepository.RemoveAllExpired(expiration);

            await _notificationRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            _logger.LogInformation("Notification cleanup performed");
        }
    }
}
