using Common.Validators;
using FluentValidation;
using MediatR;
using System;

namespace Identity.API.Application.Commands.CreateDeliveryAddress
{
    public class CreateDeliveryAddressCommand : IRequest<Guid>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public string Country { get; init; }
        public string City { get; init; }
        public string ZipCode { get; init; }
        public string Street { get; init; }
        public bool IsPrimary { get; set; }
    }

    public class CreateDeliveryAddressCommandValidator : AbstractValidator<CreateDeliveryAddressCommand>
    {
        public CreateDeliveryAddressCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .SetValidator(new FirstNameValidator());

            RuleFor(x => x.LastName)
                .SetValidator(new LastNameValidator());

            RuleFor(x => x.PhoneNumber)
                .SetValidator(new PhoneNumberValidator());

            RuleFor(x => x.Country)
                .SetValidator(new CountryValidator());

            RuleFor(x => x.City)
                .SetValidator(new CityValidator());

            RuleFor(x => x.ZipCode)
                .SetValidator(new ZipCodeValidator());

            RuleFor(x => x.Street)
                .SetValidator(new StreetValidator());
        }
    }
}
