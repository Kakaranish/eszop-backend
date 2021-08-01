using Carts.Domain.Aggregates.CartAggregate;
using Common.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Carts.Domain.Aggregates.Repositories
{
    public interface ICartRepository : IDomainRepository<Cart>
    {
        Task<Cart> GetByIdAsync(Guid id);
        Task<Cart> GetOrCreateByUserIdAsync(Guid userId);
        Task AddAsync(Cart cart);
        void Update(Cart cart);
        void Remove(Cart cart);
    }
}
