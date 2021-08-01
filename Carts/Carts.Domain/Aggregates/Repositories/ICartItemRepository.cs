using Carts.Domain.Aggregates.CartItemAggregate;
using Common.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carts.Domain.Aggregates.Repositories
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
