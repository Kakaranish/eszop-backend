using System;
using System.Collections.Generic;

namespace Common.Utilities.EventBus.IntegrationEvents
{
    public class NotificationIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public string Message { get; init; }
        public string Code { get; init; }
        public IDictionary<string, string> Metadata { get; init; }
    }
}
