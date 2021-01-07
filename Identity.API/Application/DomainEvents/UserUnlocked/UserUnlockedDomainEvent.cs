using Common.Domain;
using System;

namespace Identity.API.Application.DomainEvents.UserUnlocked
{
    public class UserUnlockedDomainEvent : IDomainEvent
    {
        public Guid UserId { get; init; }
        public DateTime UnlockedAt { get; init; }
    }
}
