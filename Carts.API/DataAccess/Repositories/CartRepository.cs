using Carts.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Common.Domain.Repositories;

namespace Carts.API.DataAccess.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public CartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<Cart> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Carts.Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cart> GetOrCreateByUserIdAsync(Guid userId)
        {
            var cart = await _appDbContext.Carts.Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart != null) return cart;

            cart = new Cart(userId);
            await AddAsync(cart);
            return cart;
        }

        public async Task AddAsync(Cart cart)
        {
            await _appDbContext.Carts.AddAsync(cart);
            await _appDbContext.SaveChangesAsync();
        }

        public void Update(Cart cart)
        {
            _appDbContext.Update(cart);
        }

        public void Remove(Cart cart)
        {
            _appDbContext.Remove(cart);
        }
    }
}
