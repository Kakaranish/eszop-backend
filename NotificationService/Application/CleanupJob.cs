using CronScheduler.Extensions.Scheduler;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Commands.RefreshCacheAndSeedUsers;
using NotificationService.DataAccess.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.Application
{
    public class CleanupJob : IScheduledJob
    {
        private readonly ILogger<CleanupJob> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationSettings _notificationSettings;
        private readonly IMediator _mediator;
        public string Name => nameof(CleanupJob);

        public CleanupJob(ILogger<CleanupJob> logger, INotificationRepository notificationRepository,
            IOptions<NotificationSettings> options, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _notificationSettings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var expiration = TimeSpan.FromMinutes(_notificationSettings.ExpirationInMinutes);
            var impactedUsers = (await _notificationRepository.RemoveAllExpired(expiration)).ToList();

            await _notificationRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            if (impactedUsers.Any())
            {
                var command = new RefreshCacheAndSeedUsersCommand { UserIds = impactedUsers };
                await _mediator.Send(command, cancellationToken);
            }

            _logger.LogInformation("Notification cleanup performed");
        }
    }
}
