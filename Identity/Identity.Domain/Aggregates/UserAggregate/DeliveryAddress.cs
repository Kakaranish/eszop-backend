using Common.Domain.Validators;
using Identity.Domain.Exceptions;
using Newtonsoft.Json;
using System;

namespace Identity.Domain.Aggregates.UserAggregate
{
    public class DeliveryAddress
    {
        [JsonProperty] public Guid Id { get; private set; }
        [JsonProperty] public string FirstName { get; private set; }
        [JsonProperty] public string LastName { get; private set; }
        [JsonProperty] public string PhoneNumber { get; private set; }
        [JsonProperty] public string Country { get; private set; }
        [JsonProperty] public string City { get; private set; }
        [JsonProperty] public string ZipCode { get; private set; }
        [JsonProperty] public string Street { get; private set; }

        [JsonConstructor]
        protected DeliveryAddress()
        {
        }

        public DeliveryAddress(string firstName, string lastName, string phoneNumber, string country,
            string city, string zipCode, string street)
        {
            Id = Guid.NewGuid();
            SetFirstName(firstName);
            SetLastName(lastName);
            SetPhoneNumber(phoneNumber);
            SetCountry(country);
            SetCity(city);
            SetZipCode(zipCode);
            SetStreet(street);
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

        public void SetCountry(string country)
        {
            ValidateCountry(country);
            Country = country;
        }

        public void SetCity(string city)
        {
            ValidateCity(city);
            City = city;
        }

        public void SetZipCode(string zipCode)
        {
            ValidateZipCode(zipCode);
            ZipCode = zipCode;
        }

        public void SetStreet(string street)
        {
            ValidateStreet(street);
            Street = street;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            ValidatePhoneNumber(phoneNumber);
            PhoneNumber = phoneNumber;
        }

        #region Validation

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

        private static void ValidateCountry(string country)
        {
            var validator = new CountryValidator();
            var result = validator.Validate(country);
            if (!result.IsValid) throw new IdentityDomainException(nameof(country));
        }

        private static void ValidateCity(string city)
        {
            var validator = new CityValidator();
            var result = validator.Validate(city);
            if (!result.IsValid) throw new IdentityDomainException(nameof(city));
        }

        private static void ValidateZipCode(string zipCode)
        {
            var validator = new ZipCodeValidator();
            var result = validator.Validate(zipCode);
            if (!result.IsValid) throw new IdentityDomainException(nameof(zipCode));
        }

        private static void ValidateStreet(string street)
        {
            var validator = new StreetValidator();
            var result = validator.Validate(street);
            if (!result.IsValid) throw new IdentityDomainException(nameof(street));
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
