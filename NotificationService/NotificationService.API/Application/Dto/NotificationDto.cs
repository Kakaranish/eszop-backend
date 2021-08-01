using System;
using System.Collections.Generic;

namespace NotificationService.API.Application.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Message { get; init; }
        public string Code { get; init; }
        public IDictionary<string, string> Metadata { get; init; }
        public bool IsRead { get; init; }
    }
}
