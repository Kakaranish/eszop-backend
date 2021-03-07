using Common.DataAccess;
using Common.Types;
using Identity.API.Application.Types;
using Identity.API.Domain;
using System;
using System.Threading.Tasks;

namespace Identity.API.DataAccess.Repositories
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
