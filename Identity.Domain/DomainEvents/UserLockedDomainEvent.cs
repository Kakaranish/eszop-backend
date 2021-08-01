using Common.Domain.DomainEvents;
using System;

namespace Identity.Domain.DomainEvents
{
    public class UserLockedDomainEvent : IDomainEvent
    {
        public Guid UserId { get; init; }
        public DateTime LockedAt { get; init; }
    }
}
