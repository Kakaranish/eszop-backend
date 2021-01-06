using Common.Domain;
using Common.Types;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Identity.API.Domain
{
    public class User : EntityBase, IAggregateRoot, ITimeStamped
    {
        private List<DeliveryAddress> _deliveryAddresses;

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string Email { get; private set; }
        public HashedPassword HashedPassword { get; private set; }
        public Role Role { get; private set; }
        public virtual ProfileInfo ProfileInfo { get; private set; }
        public virtual IReadOnlyCollection<DeliveryAddress> DeliveryAddresses =>
            _deliveryAddresses ?? new List<DeliveryAddress>();
        public Guid? PrimaryDeliveryAddressId { get; private set; }

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
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPrimaryDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            ValidatePrimaryDeliveryAddress(deliveryAddress);

            PrimaryDeliveryAddressId = deliveryAddress.Id;
            UpdatedAt = DateTime.UtcNow;
        }

        #region Validation

        private static void ValidateEmail(string email)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .EmailAddress();

            var result = validator.Validate(email);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(email)}' is invalid email");
        }

        private void ValidatePrimaryDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null) throw new IdentityDomainException(nameof(deliveryAddress));
            if (Id != deliveryAddress.UserId) throw new IdentityDomainException(nameof(deliveryAddress));
        }

        #endregion
    }
}
