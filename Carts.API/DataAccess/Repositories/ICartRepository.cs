using Carts.API.Domain;
using System;
using System.Threading.Tasks;
using Common.Domain.Repositories;

namespace Carts.API.DataAccess.Repositories
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
