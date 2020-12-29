using System;
using Carts.API.Domain;
using Common.IntegrationEvents;
using Common.Types;
using System.Threading.Tasks;

namespace Carts.API.DataAccess.Repositories
{
    public interface ICartItemRepository : IDomainRepository<CartItem>
    {
        Task<CartItem> GetByIdAsync(Guid cartItemId);
        Task UpdateWithOfferChangedEvent(OfferChangedIntegrationEvent @event);
        void Update(CartItem cartItem);
        void Remove(CartItem cartItem);
    }
}
