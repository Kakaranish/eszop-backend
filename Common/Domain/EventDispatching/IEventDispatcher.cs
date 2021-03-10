using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Domain.EventDispatching
{
    public interface IEventDispatcher
    {
        Task Dispatch(IList<EntityBase> entities);
    }
}
