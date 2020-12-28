using Common.Extensions;
using FluentValidation;
using MediatR;

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
                .IsGuid();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .When(x => x is not null);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(5)
                .When(x => x is not null);

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .When(x => x is not null);

            RuleFor(x => x.AvailableStock)
                .GreaterThanOrEqualTo(0)
                .When(x => x is not null);
        }
    }
}
