using System;
using Carts.API.Domain;
using System.Threading.Tasks;
using Common.DataAccess;
using Common.EventBus.IntegrationEvents;

namespace Carts.API.DataAccess.Repositories
{
    public interface ICartItemRepository : IDomainRepository<CartItem>
    {
        Task<CartItem> GetByIdAsync(Guid cartItemId);
        Task UpdateWithOfferChangedEvent(OfferChangedIntegrationEvent @event);
        void Update(CartItem cartItem);
        void Remove(CartItem cartItem);
        void RemoveWithOfferId(Guid offerId);
    }
}
