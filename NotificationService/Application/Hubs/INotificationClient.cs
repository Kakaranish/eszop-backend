using System.Threading.Tasks;

namespace NotificationService.Application.Hubs
{
    public interface INotificationClient
    {
        Task ReceiveNotification(Notification notification);
    }
}
