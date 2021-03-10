using Common.Domain;
using System.Collections.Generic;

namespace Common.EventBus
{
    public interface IEntityEventReducer<in T> where T : EntityBase
    {
        IEnumerable<IDomainEvent> Reduce(T entity);
    }
}
