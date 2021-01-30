using Common.Extensions;
using FluentValidation;

namespace Offers.API.Application.Commands.PublishOffer
{
    public class PublishOfferCommand
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
