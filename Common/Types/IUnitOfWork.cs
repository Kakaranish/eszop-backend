using System.Threading;
using System.Threading.Tasks;

namespace Common.Types
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default);
    }
}
