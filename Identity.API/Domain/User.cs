using Common.Domain;
using Common.Types;
using Identity.API.Application.DomainEvents.UserLocked;
using Identity.API.Domain.CommonValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.API.Domain
{
    public class User : EntityBase, IAggregateRoot, ITimeStamped
    {
        private List<DeliveryAddress> _deliveryAddresses;

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public DateTime? LockedUntil { get; private set; }
        public string Email { get; private set; }
        public HashedPassword HashedPassword { get; private set; }
        public Role Role { get; private set; }
        public virtual ProfileInfo ProfileInfo { get; private set; }
        public virtual IReadOnlyCollection<DeliveryAddress> DeliveryAddresses =>
            _deliveryAddresses ?? new List<DeliveryAddress>();
        public Guid? PrimaryDeliveryAddressId { get; private set; }
        public virtual AboutSeller AboutSeller { get; private set; }
        [NotMapped] public bool IsLocked => LockedUntil != null && LockedUntil > DateTime.UtcNow;

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

        public void SetLastLoginToNow()
        {
            LastLogin = DateTime.UtcNow;
        }

        public void SetLockedUntil(DateTime lockedUntil)
        {
            if (lockedUntil < DateTime.UtcNow)
                throw new IdentityDomainException($"'{nameof(lockedUntil)}' cannot be in past");

            LockedUntil = lockedUntil;
            UpdatedAt = DateTime.UtcNow;

            var @event = new UserLockedDomainEvent
            {
                UserId = Id,
                LockedAt = UpdatedAt,
                LockedUntil = lockedUntil
            };
            AddDomainEvent(@event);
        }

        public void SetUnlocked()
        {
            if (!IsLocked) return;

            LockedUntil = null;
            UpdatedAt = DateTime.UtcNow;
        }

        #region Validation

        private static void ValidateEmail(string email)
        {
            var validator = new EmailValidator();
            var result = validator.Validate(email);
            if (!result.IsValid) throw new IdentityDomainException(nameof(email));
        }

        private void ValidatePrimaryDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null || Id != deliveryAddress.UserId)
                throw new IdentityDomainException(nameof(deliveryAddress));
        }

        #endregion
    }
}
