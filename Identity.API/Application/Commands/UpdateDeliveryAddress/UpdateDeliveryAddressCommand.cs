using Common.Domain.Validators;
using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Identity.API.Application.Commands.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressCommand : IRequest
    {
        public string DeliveryAddressId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public string Country { get; init; }
        public string City { get; init; }
        public string ZipCode { get; init; }
        public string Street { get; init; }
        public bool IsPrimary { get; init; }
    }

    public class UpdateDeliveryAddressCommandValidator : AbstractValidator<UpdateDeliveryAddressCommand>
    {
        public UpdateDeliveryAddressCommandValidator()
        {
            RuleFor(x => x.DeliveryAddressId)
                .IsGuid();

            RuleFor(x => x.FirstName)
                .SetValidator(new FirstNameValidator())
                .When(x => x.FirstName != null);

            RuleFor(x => x.LastName)
                .SetValidator(new LastNameValidator())
                .When(x => x.LastName != null);

            RuleFor(x => x.PhoneNumber)
                .SetValidator(new PhoneNumberValidator())
                .When(x => x.PhoneNumber != null);

            RuleFor(x => x.Country)
                .SetValidator(new CountryValidator())
                .When(x => x.Country != null);

            RuleFor(x => x.City)
                .SetValidator(new CityValidator())
                .When(x => x.City != null);

            RuleFor(x => x.ZipCode)
                .SetValidator(new ZipCodeValidator())
                .When(x => x.ZipCode != null);

            RuleFor(x => x.Street)
                .SetValidator(new StreetValidator())
                .When(x => x.Street != null);
        }
    }
}
