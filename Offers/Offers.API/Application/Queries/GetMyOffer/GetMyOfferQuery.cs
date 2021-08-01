using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;
using Offers.Infrastructure.Dto;

namespace Offers.API.Application.Queries.GetMyOffer
{
    public class GetMyOfferQuery : IRequest<OfferFullViewDto>
    {
        public string OfferId { get; init; }
    }

    public class GetMyOfferQueryValidator : AbstractValidator<GetMyOfferQuery>
    {
        public GetMyOfferQueryValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();
        }
    }
}
