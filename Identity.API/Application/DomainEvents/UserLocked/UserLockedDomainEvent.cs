using Common.Domain;
using System;

namespace Identity.API.Application.DomainEvents.UserLocked
{
    public class UserLockedDomainEvent : IDomainEvent
    {
        public Guid UserId { get; init; }
        public DateTime LockedAt { get; init; }
    }
}
