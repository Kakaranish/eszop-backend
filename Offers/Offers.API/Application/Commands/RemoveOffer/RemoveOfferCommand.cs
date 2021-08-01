using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Offers.API.Application.Commands.RemoveOffer
{
    public class RemoveOfferCommand : IRequest
    {
        public string OfferId { get; init; }
    }

    public class RemoveOfferCommandValidator : AbstractValidator<RemoveOfferCommand>
    {
        public RemoveOfferCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();
        }
    }
}
