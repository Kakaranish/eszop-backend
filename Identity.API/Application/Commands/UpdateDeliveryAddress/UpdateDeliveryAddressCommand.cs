using Common.Extensions;
using Common.Validators;
using FluentValidation;
using Identity.API.Domain.CommonValidators;
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
    }

    public class UpdateDeliveryAddressCommandValidator : AbstractValidator<UpdateDeliveryAddressCommand>
    {
        public UpdateDeliveryAddressCommandValidator()
        {
            RuleFor(x => x.DeliveryAddressId)
                .IsGuid();
            
            RuleFor(x => x.FirstName)
                .SetValidator(new FirstNameValidator())
                .When(x => x.FirstName is not null);

            RuleFor(x => x.LastName)
                .SetValidator(new LastNameValidator())
                .When(x => x.LastName is not null);

            RuleFor(x => x.PhoneNumber)
                .SetValidator(new PhoneNumberValidator())
                .When(x => x.PhoneNumber is not null);

            RuleFor(x => x.Country)
                .SetValidator(new CountryValidator())
                .When(x => x.Country is not null);

            RuleFor(x => x.City)
                .SetValidator(new CityValidator())
                .When(x => x.City is not null);

            RuleFor(x => x.ZipCode)
                .SetValidator(new CityValidator())
                .When(x => x.ZipCode is not null);

            RuleFor(x => x.Street)
                .SetValidator(new StreetValidator())
                .When(x => x.Street is not null);
        }
    }
}
