using Carts.API.Domain;
using Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carts.API.DataAccess.Repositories
{
    public interface ICartItemRepository : IDomainRepository<CartItem>
    {
        Task<CartItem> GetByIdAsync(Guid cartItemId);
        Task<IList<CartItem>> GetByOfferId(Guid offerId);
        void Update(CartItem cartItem);
        void Remove(CartItem cartItem);
        void RemoveWithOfferId(Guid offerId);
    }
}
