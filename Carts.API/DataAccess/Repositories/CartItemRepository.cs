using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Carts.API.Domain;
using Common.DataAccess;
using Common.EventBus.IntegrationEvents;

namespace Carts.API.DataAccess.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _appDbContext;
        public IUnitOfWork UnitOfWork => _appDbContext;

        public CartItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<CartItem> GetByIdAsync(Guid cartItemId)
        {
            return await _appDbContext.CartItems.FirstOrDefaultAsync(x => x.Id == cartItemId);
        }

        public async Task UpdateWithOfferChangedEvent(OfferChangedIntegrationEvent @event)
        {
            await _appDbContext
                .CartItems
                .Where(cartItem => cartItem.OfferId == @event.OfferId)
                .ForEachAsync(cartItem =>
                {
                    if (@event.Price?.Changed ?? false) cartItem.SetPricePerItem((decimal)@event.Price?.NewValue.GetValueOrDefault());
                    if (@event.AvailableStock?.Changed ?? false) cartItem.SetAvailableStock((int)@event.AvailableStock?.NewValue.GetValueOrDefault());
                    if (@event.Name?.Changed ?? false) cartItem.SetOfferName(@event.Name.NewValue);
                });
        }

        public void Update(CartItem cartItem)
        {
            _appDbContext.Update(cartItem);
        }

        public void Remove(CartItem cartItem)
        {
            _appDbContext.Remove(cartItem);
        }

        public void RemoveWithOfferId(Guid offerId)
        {
            var cartItemsToRemove = _appDbContext.CartItems.Where(x => x.OfferId == offerId);
            _appDbContext.CartItems.RemoveRange(cartItemsToRemove);
        }
    }
}
