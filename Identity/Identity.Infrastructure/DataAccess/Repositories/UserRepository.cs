﻿using Common.Domain.Repositories;
using Common.Domain.Types;
using Common.Utilities.Extensions;
using Identity.Domain.Aggregates.UserAggregate;
using Identity.Domain.Repositories;
using Identity.Domain.Repositories.Types;
using Identity.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _appDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Pagination<User>> GetUsers(UserFilter filter)
        {
            var users = _appDbContext.Users
                .AsQueryable()
                .ApplyFilter(filter)
                .OrderByDescending(x => x.Role);

            var pageDetails = new PageCriteria(filter.PageIndex, filter.PageSize);

            return await users.PaginateAsync(pageDetails);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public void AddUser(User user)
        {
            _appDbContext.Add(user);
        }

        public void Update(User user)
        {
            _appDbContext.Update(user);
        }
    }
}
