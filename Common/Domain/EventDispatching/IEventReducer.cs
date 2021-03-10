using System.Collections.Generic;

namespace Common.Domain.EventDispatching
{
    public interface IEventReducer
    {
        IEnumerable<IDomainEvent> Reduce<T>(T entity) where T : EntityBase;
    }
}
