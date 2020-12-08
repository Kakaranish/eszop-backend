using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Products.API.Domain
{
    public abstract class Entity
    {
        private List<INotification> _domainEvents;

        public Guid Id { get; protected set; }
        public IReadOnlyList<INotification> DomainEvents => _domainEvents?.AsReadOnly();
        public bool IsTransient => Id == default;

        public void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents?.Add(domainEvent);
        }

        public void RemoveDomainEvent(INotification domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
