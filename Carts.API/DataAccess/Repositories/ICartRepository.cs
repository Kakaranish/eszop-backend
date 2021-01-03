using Carts.API.Domain;
using Common.DataAccess;
using System;
using System.Threading.Tasks;

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
