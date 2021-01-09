using Common.Domain;
using Common.Types;
using Common.Validators;
using Identity.API.Domain.CommonValidators;
using System;

namespace Identity.API.Domain
{
    public class SellerInfo : EntityBase, IAggregateRoot, ITimeStamped
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Guid UserId { get; private set; }
        public string ContactEmail { get; private set; }
        public string PhoneNumber { get; private set; }
        public string BankAccountNumber { get; private set; }
        public string AdditionalInfo { get; private set; }

        protected SellerInfo()
        {
        }

        public SellerInfo(Guid userId)
        {
            SetUserId(userId);

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        private void SetUserId(Guid userId)
        {
            ValidateUserId(userId);

            UserId = userId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetContactEmail(string email)
        {
            ValidateContactEmail(email);

            ContactEmail = !string.IsNullOrWhiteSpace(email) ? email : null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);

            PhoneNumber = !string.IsNullOrWhiteSpace(phoneNumber) ? phoneNumber : null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAdditionalInfo(string additionalInfo)
        {
            AdditionalInfo = !string.IsNullOrWhiteSpace(additionalInfo) ? additionalInfo : null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetBankAccountNumber(string bankAccountNumber)
        {
            ValidateBankAccountNumber(bankAccountNumber);

            BankAccountNumber = !string.IsNullOrWhiteSpace(bankAccountNumber) ? bankAccountNumber : null;
            UpdatedAt = DateTime.UtcNow;
        }

        #region Validation

        private static void ValidateUserId(Guid userId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(userId);
            if (!result.IsValid) throw new IdentityDomainException(nameof(userId));
        }

        private static void ValidateContactEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return;

            var validator = new EmailValidator();
            var result = validator.Validate(email);
            if (!result.IsValid) throw new IdentityDomainException(nameof(email));
        }

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return;

            var validator = new PhoneNumberValidator();
            var result = validator.Validate(phoneNumber);
            if (!result.IsValid) throw new IdentityDomainException(nameof(phoneNumber));
        }

        private static void ValidateBankAccountNumber(string bankAccountNumber)
        {
            if (string.IsNullOrEmpty(bankAccountNumber)) return;

            var validator = new BankAccountNumberValidator();
            var result = validator.Validate(bankAccountNumber);
            if (!result.IsValid) throw new IdentityDomainException(nameof(bankAccountNumber));
        }

        #endregion
    }
}
