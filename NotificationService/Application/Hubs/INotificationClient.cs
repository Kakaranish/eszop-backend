using NotificationService.Application.Dto;
using System.Threading.Tasks;

namespace NotificationService.Application.Hubs
{
    public interface INotificationClient
    {
        Task ReceiveNotification(NotificationDto notification);
        Task ClearNotifications();
    }
}
