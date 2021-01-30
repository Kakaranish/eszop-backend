using Common.Types;
using FluentValidation;
using MediatR;
using Offers.API.Application.Dto;
using Offers.API.Application.Types;

namespace Offers.API.Application.Queries.GetActiveOffers
{
    public class GetActiveOffersQuery : IRequest<Pagination<OfferDto>>
    {
        public OfferFilter OfferFilter { get; init; }
    }

    public class GetActiveOffersQueryValidator : AbstractValidator<GetActiveOffersQuery>
    {
        public GetActiveOffersQueryValidator()
        {
            RuleFor(x => x.OfferFilter)
                .SetValidator(new OfferFilterValidator());
        }
    }
}
