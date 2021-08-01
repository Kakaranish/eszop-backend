using Common.Domain.Repositories;
using Common.Domain.Types;
using Identity.Domain.Aggregates.UserAggregate;
using Identity.Domain.Repositories.Types;
using System;
using System.Threading.Tasks;

namespace Identity.Domain.Repositories
{
    public interface IUserRepository : IDomainRepository<User>
    {
        Task<Pagination<User>> GetUsers(UserFilter filter);
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        void AddUser(User user);
        void Update(User user);
    }
}
