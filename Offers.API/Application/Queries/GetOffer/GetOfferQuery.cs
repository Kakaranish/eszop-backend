using System;
using FluentValidation;
using MediatR;
using Offers.API.Domain;

namespace Offers.API.Application.Queries.GetOffer
{
    public class GetOfferQuery : IRequest<Offer>
    {
        public string OfferId { get; }

        public GetOfferQuery(string offerId)
        {
            OfferId = offerId;
        }
    }

    public class GetOfferQueryValidator : AbstractValidator<GetOfferQuery>
    {
        public GetOfferQueryValidator()
        {
            RuleFor(x => x.OfferId)
                .NotNull()
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("Invalid guid");
        }
    }
}
