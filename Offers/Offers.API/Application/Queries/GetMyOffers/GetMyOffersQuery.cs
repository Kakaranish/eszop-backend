using Common.Domain.Types;
using FluentValidation;
using MediatR;
using Offers.Domain.Repositories.Types;
using Offers.Infrastructure.Dto;

namespace Offers.API.Application.Queries.GetMyOffers
{
    public class GetMyOffersQuery : IRequest<Pagination<OfferListPreviewDto>>
    {
        public OfferFilter OfferFilter { get; init; }
    }

    public class GetMyOffersQueryValidator : AbstractValidator<GetMyOffersQuery>
    {
        public GetMyOffersQueryValidator()
        {
            RuleFor(x => x.OfferFilter)
                .SetValidator(new OfferFilterValidator());
        }
    }
}
