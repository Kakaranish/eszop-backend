using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Offers.API.Application.Commands.EndOffer
{
    public class EndOfferCommand : IRequest
    {
        public string OfferId { get; init; }
    }

    public class EndOfferCommandValidator : AbstractValidator<EndOfferCommand>
    {
        public EndOfferCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();
        }
    }
}
