using Common.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.EventBus
{
    public interface IEventDispatcher
    {
        Task Dispatch(IList<EntityBase> entities);
    }
}
