using System;

namespace Common.Utilities.EventBus.IntegrationEvents
{
    public class UserLockedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public DateTime LockedAt { get; init; }
    }
}
