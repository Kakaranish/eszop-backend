using System;
using System.Threading.Tasks;
using Common.DataAccess;
using Orders.API.Domain;

namespace Orders.API.DataAccess.Repositories
{
    public interface IOrderRepository : IDomainRepository<Order>
    {
        Task<Order> GetByIdAsync(Guid orderId);
        void Add(Order order);
        void Update(Order order);
    }
}
