using Common.DataAccess;
using Common.Types;
using Orders.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orders.API.DataAccess.Repositories
{
    public interface IOrderRepository : IDomainRepository<Order>
    {
        Task<Order> GetByIdAsync(Guid orderId);
        Task<Pagination<Order>> GetAllByUserIdAsync(Guid userId, BasicPaginationFilter filter);
        Task<IList<Order>> GetAllStartedOrdersByOfferId(Guid offerId);
        void Add(Order order);
        void Update(Order order);
    }
}
