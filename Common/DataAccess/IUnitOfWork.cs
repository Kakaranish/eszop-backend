using System.Threading;
using System.Threading.Tasks;

namespace Common.DataAccess
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAndDispatchDomainEventsAsync(CancellationToken cancellationToken = default);
    }
}
