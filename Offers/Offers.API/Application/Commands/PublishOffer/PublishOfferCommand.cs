using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Offers.API.Application.Commands.PublishOffer
{
    public class PublishOfferCommand : IRequest
    {
        public string OfferId { get; init; }
    }

    public class PublishOfferCommandValidator : AbstractValidator<PublishOfferCommand>
    {
        public PublishOfferCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();
        }
    }
}
