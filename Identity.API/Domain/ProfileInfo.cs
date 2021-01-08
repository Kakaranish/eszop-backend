using Common.Domain;
using Common.Validators;
using Identity.API.Domain.CommonValidators;
using System;

namespace Identity.API.Domain
{
    public class ProfileInfo : EntityBase, IAggregateRoot
    {
        public virtual User User { get; private set; }
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string PhoneNumber { get; private set; }

        public ProfileInfo(Guid userId, string firstName, string lastName, DateTime dateOfBirth, string phoneNumber)
        {
            SetUserId(userId);
            SetFirstName(firstName);
            SetLastName(lastName);
            SetDateOfBirth(dateOfBirth);
            SetPhoneNumber(phoneNumber);
        }

        private void SetUserId(Guid userId)
        {
            ValidateUserId(userId);
            UserId = userId;
        }

        public void SetFirstName(string firstName)
        {
            ValidateFirstName(firstName);
            if (firstName == FirstName) return;
            
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            ValidateLastName(lastName);
            if (lastName == LastName) return;

            LastName = lastName;
        }

        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            ValidateDateOfBirth(dateOfBirth);
            if (dateOfBirth == DateOfBirth) return;

            DateOfBirth = dateOfBirth;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);
            if (phoneNumber == PhoneNumber) return;

            PhoneNumber = phoneNumber;
        }

        #region Validation

        private static void ValidateUserId(Guid userId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(userId);
            if (!result.IsValid) throw new IdentityDomainException(nameof(userId));
        }

        private static void ValidateFirstName(string firstName)
        {
            var validator = new FirstNameValidator();
            var result = validator.Validate(firstName);
            if (!result.IsValid) throw new IdentityDomainException(nameof(firstName));
        }

        private static void ValidateLastName(string lastName)
        {
            var validator = new LastNameValidator();
            var result = validator.Validate(lastName);
            if (!result.IsValid) throw new IdentityDomainException(nameof(lastName));
        }

        private static void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            var validator = new DateOfBirthValidator();
            var result = validator.Validate(dateOfBirth);
            if (!result.IsValid) throw new IdentityDomainException(nameof(dateOfBirth));
        }

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            var validator = new PhoneNumberValidator();
            var result = validator.Validate(phoneNumber);
            if (!result.IsValid) throw new IdentityDomainException(nameof(phoneNumber));
        }

        #endregion
    }
}
