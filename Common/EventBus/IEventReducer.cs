using Common.Domain;
using System.Collections.Generic;

namespace Common.EventBus
{
    public interface IEventReducer
    {
        IEnumerable<IDomainEvent> Reduce<T>(T entity) where T : EntityBase;
    }
}
