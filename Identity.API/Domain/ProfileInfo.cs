using Common.Domain;
using Common.Validators;
using FluentValidation;
using System;
using Identity.API.Domain.CommonValidators;

namespace Identity.API.Domain
{
    public class ProfileInfo : EntityBase, IAggregateRoot
    {
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

        public void SetUserId(Guid userId)
        {
            ValidateUserId(userId);
            UserId = userId;
        }

        public void SetFirstName(string firstName)
        {
            ValidateFirstName(firstName);
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            ValidateLastName(lastName);
            LastName = lastName;
        }

        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            ValidateDateOfBirth(dateOfBirth);
            DateOfBirth = dateOfBirth;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);
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
            var minDate = new DateTime(1930, 1, 1);
            var validator = new InlineValidator<DateTime>();
            validator.RuleFor(x => x)
                .Must(x => x > minDate);

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
