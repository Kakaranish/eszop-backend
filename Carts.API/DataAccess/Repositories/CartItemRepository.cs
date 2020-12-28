using Common.IntegrationEvents;
using Common.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task UpdateWithOfferChangedEvent(OfferChangedIntegrationEvent @event)
        {
            await _appDbContext
                .CartItems
                .Where(cartItem => cartItem.OfferId == @event.OfferId)
                .ForEachAsync(cartItem =>
                {
                    if (@event.Price.Changed) cartItem.SetPricePerItem((decimal)@event.Price?.NewValue.GetValueOrDefault());
                    if (@event.Name.Changed) cartItem.SetOfferName(@event.Name.NewValue);
                });
        }
    }
}
