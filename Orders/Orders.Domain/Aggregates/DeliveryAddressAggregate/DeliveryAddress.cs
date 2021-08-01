using Common.Domain;
using Common.Domain.Types;
using Common.Domain.Validators;
using Orders.Domain.Exceptions;
using System;

namespace Orders.Domain.Aggregates.DeliveryAddressAggregate
{
    public class DeliveryAddress : EntityBase, IAggregateRoot, ITimeStamped
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string Street { get; private set; }

        public DeliveryAddress(string firstName, string lastName, string phoneNumber, string country,
            string city, string zipCode, string street)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetPhoneNumber(phoneNumber);
            SetCountry(country);
            SetCity(city);
            SetZipCode(zipCode);
            SetStreet(street);

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public void SetFirstName(string firstName)
        {
            ValidateFirstName(firstName);

            FirstName = firstName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetLastName(string lastName)
        {
            ValidateLastName(lastName);

            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCountry(string country)
        {
            ValidateCountry(country);

            Country = country;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCity(string city)
        {
            ValidateCity(city);

            City = city;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetZipCode(string zipCode)
        {
            ValidateZipCode(zipCode);

            ZipCode = zipCode;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetStreet(string street)
        {
            ValidateStreet(street);

            Street = street;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);

            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        #region Validation

        private static void ValidateFirstName(string firstName)
        {
            var validator = new FirstNameValidator();
            var result = validator.Validate(firstName);
            if (!result.IsValid) throw new OrdersDomainException(nameof(firstName));
        }

        private static void ValidateLastName(string lastName)
        {
            var validator = new LastNameValidator();
            var result = validator.Validate(lastName);
            if (!result.IsValid) throw new OrdersDomainException(nameof(lastName));
        }

        private static void ValidateCountry(string country)
        {
            var validator = new CountryValidator();
            var result = validator.Validate(country);
            if (!result.IsValid) throw new OrdersDomainException(nameof(country));
        }

        private static void ValidateCity(string city)
        {
            var validator = new CityValidator();
            var result = validator.Validate(city);
            if (!result.IsValid) throw new OrdersDomainException(nameof(city));
        }

        private static void ValidateZipCode(string zipCode)
        {
            var validator = new ZipCodeValidator();
            var result = validator.Validate(zipCode);
            if (!result.IsValid) throw new OrdersDomainException(nameof(zipCode));
        }

        private static void ValidateStreet(string street)
        {
            var validator = new StreetValidator();
            var result = validator.Validate(street);
            if (!result.IsValid) throw new OrdersDomainException(nameof(street));
        }

        private static void ValidatePhoneNumber(string phoneNumber)
        {
            var validator = new PhoneNumberValidator();
            var result = validator.Validate(phoneNumber);
            if (!result.IsValid) throw new OrdersDomainException(nameof(phoneNumber));
        }

        #endregion
    }
}
