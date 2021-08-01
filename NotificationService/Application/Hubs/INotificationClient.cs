using NotificationService.API.Application.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.API.Application.Hubs
{
    public interface INotificationClient
    {
        Task SeedNotifications(IEnumerable<NotificationDto> notifications);
        Task ReceiveNotification(NotificationDto notification);
    }
}
