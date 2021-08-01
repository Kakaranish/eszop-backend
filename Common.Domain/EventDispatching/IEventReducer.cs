using System.Collections.Generic;
using Common.Domain.DomainEvents;

namespace Common.Domain.EventDispatching
{
    public interface IEventReducer
    {
        IEnumerable<IDomainEvent> Reduce<T>(T entity) where T : EntityBase;
    }
}
