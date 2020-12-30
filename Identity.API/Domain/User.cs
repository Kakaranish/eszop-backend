using FluentValidation;
using System;
using Common.Domain;

namespace Identity.API.Domain
{
    public class User : EntityBase, IAggregateRoot
    {
        public string Email { get; private set; }
        public HashedPassword HashedPassword { get; private set; }
        public Role Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected User()
        {
        }

        public User(string email, HashedPassword hashedPassword, Role role)
        {
            ValidateEmail(email);
            Email = email;

            HashedPassword = hashedPassword ?? throw new IdentityDomainException($"'{nameof(hashedPassword)}' cannot be null");

            Role = role ?? throw new IdentityDomainException($"'{nameof(hashedPassword)}' cannot be null");

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(HashedPassword newPassword)
        {
            HashedPassword = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
        }

        private static void ValidateEmail(string email)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .EmailAddress();

            var result = validator.Validate(email);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(email)}' is invalid email");
        }
    }
}
