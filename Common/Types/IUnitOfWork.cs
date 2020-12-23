using System.Threading;
using System.Threading.Tasks;

namespace Common.Types
{
    public interface IUnitOfWork
    {
        Task SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default);
    }
}
