using CronScheduler.Extensions.Scheduler;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.API.Application.Commands.RefreshCacheAndSeedUsers;
using NotificationService.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationService.API.Application
{
    public class CleanupJob : IScheduledJob
    {
        private readonly ILogger<CleanupJob> _logger;
        private readonly NotificationSettings _notificationSettings;
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        public string Name => nameof(CleanupJob);

        public CleanupJob(ILogger<CleanupJob> logger, IOptions<NotificationSettings> options,
            IMediator mediator, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notificationSettings = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

            var expiration = TimeSpan.FromMinutes(_notificationSettings.ExpirationInMinutes);
            var impactedUsers = (await notificationRepository.RemoveAllExpired(expiration)).ToList();

            await notificationRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            if (impactedUsers.Any())
            {
                var command = new RefreshCacheAndSeedUsersCommand { UserIds = impactedUsers };
                await _mediator.Send(command, cancellationToken);
            }

            _logger.LogInformation("Notification cleanup performed");
        }
    }
}
