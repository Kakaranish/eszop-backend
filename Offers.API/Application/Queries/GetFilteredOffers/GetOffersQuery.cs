using Common.Types;
using FluentValidation;
using MediatR;
using Offers.API.Application.Dto;
using Offers.API.Application.Types;

namespace Offers.API.Application.Queries.GetFilteredOffers
{
    public class GetOffersQuery : IRequest<Pagination<OfferDto>>
    {
        public OfferFilter OfferFilter { get; init; }
    }

    public class GetOffersQueryValidator : AbstractValidator<GetOffersQuery>
    {
        public GetOffersQueryValidator()
        {
            RuleFor(x => x.OfferFilter)
                .SetValidator(new OfferFilterValidator());
        }
    }
}
