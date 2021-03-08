using System;
using System.Collections.Generic;

namespace Common.EventBus.IntegrationEvents
{
    public class NotificationIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string> Details { get; set; }
    }
}
