using System.Threading.Tasks;
using Common.DataAccess;
using Orders.API.Domain;

namespace Orders.API.DataAccess.Repositories
{
    public interface IOrderRepository : IDomainRepository<Order>
    {
        Task AddAsync(Order order);
    }
}
