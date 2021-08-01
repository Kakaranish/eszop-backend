using Identity.Domain.Aggregates.UserAggregate;

namespace Identity.API.Services
{
    public interface IPasswordHasher
    {
        HashedPassword Hash(string plaintextPassword);
        bool Verify(string plaintextPassword, HashedPassword hashedPasswordToCompare);
    }
}