using Common.Types;
using Orders.API.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Domain.Repositories;
using Common.Domain.Types;

namespace Orders.API.DataAccess.Repositories
{
    public interface IOrderRepository : IDomainRepository<Order>
    {
        Task<Order> GetByIdAsync(Guid orderId);
        Task<Pagination<Order>> GetAllByBuyerIdAsync(Guid userId, BasicPaginationFilter filter);
        Task<Pagination<Order>> GetAllBySellerIdAsync(Guid userId, BasicPaginationFilter filter);
        Task<IList<Order>> GetAllStartedOrdersByOfferId(Guid offerId);
        Task<bool> GetOfferHasAnyOrders(Guid offerId);
        void Add(Order order);
        void Update(Order order);
    }
}
