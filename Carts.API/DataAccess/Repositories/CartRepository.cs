using Carts.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Types;

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

        public async Task<IList<Cart>> GetAllAsync()
        {
            return await _appDbContext.Carts.ToListAsync();
        }

        public async Task<Cart> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Carts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cart> GetOrCreateByUserIdAsync(Guid userId)
        {
            var cart = await _appDbContext.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart is not null) return cart;
            
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

        public void Delete(Cart cart)
        {
            _appDbContext.Remove(cart);
        }
    }
}
