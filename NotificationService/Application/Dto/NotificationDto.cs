using System;
using System.Collections.Generic;

namespace NotificationService.Application.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Message { get; init; }
        public IDictionary<string, string> Details { get; init; }
        public bool IsRead { get; init; }
        public bool MarkedAsDeleted { get; init; }
    }
}
