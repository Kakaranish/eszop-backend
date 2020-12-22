using System;
using Common.Types;
using Common.Types.Domain;

namespace Identity.API.Domain
{
    public class RefreshToken : EntityBase, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        public bool IsRevoked => RevokedAt.HasValue;

        protected RefreshToken()
        {
        }

        public RefreshToken(Guid userId, string token)
        {
            UserId = userId != Guid.Empty
                ? userId
                : throw new ArgumentException(nameof(userId));

            Token = !string.IsNullOrWhiteSpace(token)
                ? token
                : throw new ArgumentNullException(nameof(token));

            CreatedAt = DateTime.UtcNow;
        }

        public void Revoke()
        {
            if (IsRevoked)
            {
                throw new DomainException($"Refresh token with id '{Id}' is already revoked");
            }

            RevokedAt = DateTime.UtcNow;
        }
    }
}
