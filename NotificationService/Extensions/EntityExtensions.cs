using NotificationService.API.Application.Dto;
using NotificationService.Domain.Aggregates.NotificationAggregate;

namespace NotificationService.API.Extensions
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
                Code = notification.Code,
                Metadata = notification.Metadata,
                IsRead = notification.IsRead
            };
        }
    }
}
