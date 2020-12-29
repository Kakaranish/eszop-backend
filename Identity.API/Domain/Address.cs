using FluentValidation;

namespace Identity.API.Domain
{
    public class Address
    {
        public string City { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
        public string HouseNumber { get; private set; }

        public Address(string city, string street, string zipCode, string houseNumber)
        {
            ValidateCity(city);
            ValidateStreet(street);
            ValidateCity(zipCode);
            ValidateHouseNumber(houseNumber);

            City = city;
            Street = street;
            ZipCode = zipCode;
            HouseNumber = houseNumber;
        }

        public void ValidateCity(string city)
        {
            const string cityRegex = @"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$";
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .Matches(cityRegex);

            var result = validator.Validate(city);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(city)}' is invalid city");
        }

        public void ValidateStreet(string street)
        {
            const string streetRegex = @"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$";
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .Matches(streetRegex);

            var result = validator.Validate(streetRegex);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(street)}' is invalid street");
        }

        public void ValidateZipCode(string zipCode)
        {
            const string zipCodeRegex = @"^\d{2}-\d{3}$";
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .Matches(zipCodeRegex);

            var result = validator.Validate(zipCode);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(zipCode)}' is invalid zip code");
        }

        public void ValidateHouseNumber(string houseNumber)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .NotEmpty();

            var result = validator.Validate(houseNumber);
            if (!result.IsValid) throw new IdentityDomainException($"'{nameof(houseNumber)}' is invalid house number");
        }
    }
}
