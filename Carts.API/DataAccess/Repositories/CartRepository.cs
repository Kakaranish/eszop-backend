﻿using Carts.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carts.API.DataAccess.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _appDbContext;

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

        public async Task AddAsync(Cart cart)
        {
            await _appDbContext.Carts.AddAsync(cart);
            await _appDbContext.SaveChangesAsync();
        }
    }
}