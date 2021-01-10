using Common.Extensions;
using Common.Validators;
using FluentValidation;
using MediatR;
using Offers.API.Domain.Validators;

namespace Offers.API.Application.Commands.UpdateOffer
{
    public class UpdateOfferCommand : IRequest
    {
        public string OfferId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal? Price { get; init; }
        public int? AvailableStock { get; init; }
    }

    public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
    {
        public UpdateOfferCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();

            RuleFor(x => x.Name)
                .SetValidator(new OfferNameValidator())
                .When(x => x.Name is not null);

            RuleFor(x => x.Description)
                .SetValidator(new OfferDescriptionValidator())
                .When(x => x.Description is not null);

            RuleFor(x => x.Price)
                .Must(price =>
                {
                    if (price == null) return true;
                    var validator = new OfferPriceValidator();
                    return validator.Validate(price.Value).IsValid;
                })
                .When(x => x.Price is not null);

            RuleFor(x => x.AvailableStock)
                .GreaterThanOrEqualTo(0)
                .When(x => x.AvailableStock is not null);
        }
    }
}
