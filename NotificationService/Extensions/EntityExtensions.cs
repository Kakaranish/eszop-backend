using NotificationService.Application.Domain;
using NotificationService.Application.Dto;

namespace NotificationService.Extensions
{
    public static class EntityExtensions
    {
        public static NotificationDto ToDto(this Notification notification)
        {
            if (notification == null) return null;

            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                CreatedAt = notification.CreatedAt,
                Message = notification.Message,
                Details = notification.Details,
                IsRead = notification.IsRead
            };
        }
    }
}
