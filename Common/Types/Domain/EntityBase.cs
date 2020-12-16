using System;
using System.Collections.Generic;

namespace Common.Types
{
    public abstract class EntityBase
    {
        private List<IDomainEvent> _domainEvents;

        public Guid Id { get; protected set; }
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();
        public bool IsTransient => Id == default;

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents?.Add(domainEvent);
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
