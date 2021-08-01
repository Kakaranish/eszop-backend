using Identity.Domain.Aggregates.UserAggregate;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace Identity.API.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int IterationCount = 10000;
        private const int SaltSize = 128 / 8;
        private const int HashSize = 256 / 8;

        public HashedPassword Hash(string plaintextPassword)
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Hash(plaintextPassword, salt);
        }

        public HashedPassword Hash(string plaintextPassword, byte[] salt, int iterationCount = IterationCount)
        {
            if (salt?.Length == 0) throw new ArgumentException(nameof(salt));

            return InnerHash(plaintextPassword, salt, iterationCount);
        }

        public bool Verify(string plaintextPassword, HashedPassword hashedPasswordToCompare)
        {
            var hashedPassword = InnerHash(plaintextPassword, hashedPasswordToCompare.Salt, hashedPasswordToCompare.IterationCount);
            return hashedPassword == hashedPasswordToCompare;
        }

        private static HashedPassword InnerHash(string plaintextPassword, byte[] salt, int iterationCount = IterationCount)
        {
            var hashedPassword = KeyDerivation.Pbkdf2(
                password: plaintextPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: iterationCount,
                numBytesRequested: HashSize);

            return new HashedPassword(hashedPassword, salt, IterationCount);
        }
    }
}
