using System;
using System.Linq;

namespace Identity.API.Domain
{
    public class HashedPassword : IEquatable<HashedPassword>
    {
        public byte[] Hash { get; }
        public byte[] Salt { get; }
        public int IterationCount { get; }

        public HashedPassword(string hashedPassword)
        {
            var splitPassword = hashedPassword.Split('|');
            if (splitPassword.Length != 3 || !int.TryParse(splitPassword[2], out var iterationCount))
            {
                throw new InvalidHashedPasswordFormatException(nameof(hashedPassword));
            }

            Hash = Convert.FromBase64String(splitPassword[0]);
            Salt = Convert.FromBase64String(splitPassword[1]);
            IterationCount = iterationCount;
        }

        public HashedPassword(byte[] hash, byte[] salt, int iterationCount)
        {
            if (iterationCount <= 0)
            {
                throw new ArgumentException($"{nameof(iterationCount)} must be greater than 0");
            }

            Hash = hash;
            Salt = salt;
            IterationCount = iterationCount;
        }

        public override string ToString()
        {
            var hashAsString = Convert.ToBase64String(Hash);
            var saltAsString = Convert.ToBase64String(Salt);

            return $"{hashAsString}|{saltAsString}|{IterationCount}";
        }

        public bool Equals(HashedPassword other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return (Hash?.SequenceEqual(other.Hash) ?? false) &&
                   (Salt?.SequenceEqual(other.Salt) ?? false) && 
                   IterationCount == other.IterationCount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HashedPassword)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hash, Salt, IterationCount);
        }

        public static bool operator ==(HashedPassword obj1, HashedPassword obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }
            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(HashedPassword obj1, HashedPassword obj2)
        {
            return !(obj1 == obj2);
        }

        private class InvalidHashedPasswordFormatException : Exception
        {
            public InvalidHashedPasswordFormatException(string argName) : base($"{argName} has invalid format")
            {
            }
        }
    }
}
