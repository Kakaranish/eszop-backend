using Common.Domain;
using System.Collections.Generic;

namespace Common.EventBus
{
    public class DefaultEventReducer : IEventReducer
    {
        public IEnumerable<IDomainEvent> Reduce<T>(T entity) where T : EntityBase
        {
            return entity.DomainEvents;
        }
    }
}
