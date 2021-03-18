using System;

namespace Common.EventBus.IntegrationEvents
{
    public class UserLockedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public DateTime LockedAt { get; init; }
    }
}
