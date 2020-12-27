using Common.Extensions;
using FluentValidation;
using MediatR;
using Offers.API.Domain;

namespace Offers.API.Application.Queries.GetOffer
{
    public class GetOfferQuery : IRequest<Offer>
    {
        public string OfferId { get; init; }
    }

    public class GetOfferQueryValidator : AbstractValidator<GetOfferQuery>
    {
        public GetOfferQueryValidator()
        {
            RuleFor(x => x.OfferId)
                .NotNull()
                .IsGuid();
        }
    }
}
