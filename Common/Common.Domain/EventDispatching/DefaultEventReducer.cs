using System.Collections.Generic;
using Common.Domain.DomainEvents;

namespace Common.Domain.EventDispatching
{
    public class DefaultEventReducer : IEventReducer
    {
        public IEnumerable<IDomainEvent> Reduce<T>(T entity) where T : EntityBase
        {
            return entity.DomainEvents;
        }
    }
}
