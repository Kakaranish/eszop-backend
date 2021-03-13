using NotificationService.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Application.Hubs
{
    public interface INotificationClient
    {
        Task SeedNotifications(IEnumerable<NotificationDto> notifications);
        Task ReceiveNotification(NotificationDto notification);
        Task ClearNotifications();
    }
}
