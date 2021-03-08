using System;
using System.Collections.Generic;

namespace NotificationService.Application
{
    public class Notification
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? ExpiresOn { get; init; }
        public string Message { get; init; }
        public IDictionary<string, string> Details { get; init; }
    }
}
