using Common.Domain;
using Common.Types;
using Common.Validators;
using FluentValidation;
using Identity.API.Domain.CommonValidators;
using System;

namespace Identity.API.Domain
{
    public class AboutSeller : EntityBase, IAggregateRoot, ITimeStamped
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Guid UserId { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Description { get; private set; }

        protected AboutSeller()
        {
        }

        public AboutSeller(Guid userId, string email, string phoneNumber, string description)
        {
            SetUserId(userId);
            SetEmail(email);
            SetPhoneNumber(phoneNumber);
            SetDescription(description);

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public void SetUserId(Guid userId)
        {
            ValidateUserId(userId);
            UserId = userId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetEmail(string email)
        {
            ValidateEmail(email);
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            ValidateDescription(description);
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        #region Validation

        private static void ValidateUserId(Guid userId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(userId);
            if (!result.IsValid) throw new IdentityDomainException(nameof(userId));
        }

        private static void ValidateEmail(string email)
        {
            var validator = new EmailValidator();
            var result = validator.Validate(email);
            if (!result.IsValid) throw new IdentityDomainException(nameof(email));
        }

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            var validator = new PhoneNumberValidator();
            var result = validator.Validate(phoneNumber);
            if (!result.IsValid) throw new IdentityDomainException(nameof(phoneNumber));
        }

        private static void ValidateDescription(string description)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);

            var result = validator.Validate(description);
            if (!result.IsValid) throw new IdentityDomainException(nameof(description));
        }

        #endregion
    }
}
