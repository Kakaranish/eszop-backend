using Carts.Domain.Aggregates.CartItemAggregate;
using Carts.Domain.Aggregates.Repositories;
using Common.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carts.Infrastructure.DataAccess.Repositories
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

        public async Task<IList<CartItem>> GetByOfferId(Guid offerId)
        {
            return await _appDbContext
                .CartItems
                .Where(cartItem => cartItem.OfferId == offerId)
                .ToListAsync();
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
