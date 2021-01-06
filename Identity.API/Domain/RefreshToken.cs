using System;
using Common.Domain;
using Common.Validators;

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
            SetUserId(userId);
            SetToken(token);
            CreatedAt = DateTime.UtcNow;
        }

        private void SetUserId(Guid userId)
        {
            ValidateUserId(userId);
            UserId = userId;
        }

        private void SetToken(string token)
        {
            ValidateToken(token);
            Token = token;
        }

        public void Revoke()
        {
            if (IsRevoked)
            {
                throw new IdentityDomainException($"Token {Id} is already revoked");
            }

            RevokedAt = DateTime.UtcNow;
        }

        #region Validation

        private static void ValidateUserId(Guid userId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(userId);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(userId)}' cannot be empty guid");
        }

        private static void ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new IdentityDomainException(nameof(token));
        }

        #endregion
    }
}
