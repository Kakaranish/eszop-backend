using Common.Domain;
using Common.Types;
using Common.Validators;
using Identity.API.Application.DomainEvents.UserLocked;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        public Guid? PrimaryDeliveryAddressId { get; private set; }
        public IReadOnlyCollection<DeliveryAddress> DeliveryAddresses =>
            _deliveryAddresses ?? new List<DeliveryAddress>();
        public virtual SellerInfo SellerInfo { get; private set; }
        
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

        public void AddDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            ValidateAddDeliveryAddress(deliveryAddress);

            _deliveryAddresses ??= new List<DeliveryAddress>();
            _deliveryAddresses.Add(deliveryAddress);

            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveDeliveryAddress(Guid deliveryAddressId)
        {
            ValidateRemoveDeliveryAddress(deliveryAddressId);

            _deliveryAddresses.Remove(_deliveryAddresses.FirstOrDefault(x => x.Id == deliveryAddressId));

            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPrimaryDeliveryAddress(Guid deliveryAddressId)
        {
            ValidateSetPrimaryAddressId(deliveryAddressId);

            PrimaryDeliveryAddressId = deliveryAddressId;

            UpdatedAt = DateTime.UtcNow;
        }

        public void UnsetPrimaryDeliveryAddress()
        {
            PrimaryDeliveryAddressId = null;
            
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

        private static void ValidateAddDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null) throw new IdentityDomainException($"{nameof(deliveryAddress)} cannot be null");
        }

        private void ValidateRemoveDeliveryAddress(Guid deliveryAddressId)
        {
            if (deliveryAddressId == Guid.Empty)
                throw new IdentityDomainException($"'{nameof(deliveryAddressId)}' cannot be empty guid");

            var toRemove = _deliveryAddresses?.FirstOrDefault(x => x.Id == deliveryAddressId);
            if (toRemove == null)
                throw new IdentityDomainException($"There is no delivery address {deliveryAddressId} to remove");
        }

        private void ValidateSetPrimaryAddressId(Guid deliveryAddressId)
        {
            if (deliveryAddressId == Guid.Empty)
                throw new IdentityDomainException($"'{nameof(deliveryAddressId)}' cannot be empty guid");

            if (_deliveryAddresses == null || _deliveryAddresses.Count == 0)
                throw new IdentityDomainException("There are no delivery addresses");

            if (_deliveryAddresses.All(address => address.Id != deliveryAddressId))
                throw new IdentityDomainException($"There is no delivery address with id {deliveryAddressId}");
        }

        #endregion
    }
}
