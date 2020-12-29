using Common.Types.Domain;
using FluentValidation;
using System;

namespace Identity.API.Domain
{
    public class ProfileInfo : EntityBase, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string PhoneNumber { get; private set; }

        public ProfileInfo(string firstName, string lastName, DateTime dateOfBirth, string phoneNumber)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateDateOfBirth(dateOfBirth);
            ValidatePhoneNumber(phoneNumber);

            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
        }

        private static void ValidateFirstName(string firstName)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);

            var result = validator.Validate(firstName);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(firstName)}' must have at least 3 characters");
        }

        private static void ValidateLastName(string lastName)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);

            var result = validator.Validate(lastName);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(lastName)}' must have at least 3 characters");
        }

        private static void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            var minDate = new DateTime(1930, 1, 1);
            var validator = new InlineValidator<DateTime>();
            validator.RuleFor(x => x)
                .Must(x => x > minDate);

            var result = validator.Validate(dateOfBirth);
            if (!result.IsValid) throw new IdentityDomainException($"Min '{nameof(dateOfBirth)}' is {minDate:yyyy-MM-dd}");
        }

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .Matches(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)");

            var result = validator.Validate(phoneNumber);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(phoneNumber)}' is invalid polish phone number");
        }
    }
}
