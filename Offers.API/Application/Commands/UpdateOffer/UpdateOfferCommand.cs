using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Offers.API.Application.Commands.UpdateOffer
{
    public class UpdateOfferCommand : IRequest
    {
        public string OfferId { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal? Price { get; }

        public UpdateOfferCommand(string offerId, string name, string description, decimal? price)
        {
            OfferId = offerId;
            Name = name;
            Description = description;
            Price = price;
        }
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

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .When(x => x is not null);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(5)
                .When(x => x is not null);
        }
    }
}
