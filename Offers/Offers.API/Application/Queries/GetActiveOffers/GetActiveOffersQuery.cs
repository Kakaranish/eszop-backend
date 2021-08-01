using Common.Domain.Types;
using FluentValidation;
using MediatR;
using Offers.Domain.Repositories.Types;
using Offers.Infrastructure.Dto;

namespace Offers.API.Application.Queries.GetActiveOffers
{
    public class GetActiveOffersQuery : IRequest<Pagination<OfferListPreviewDto>>
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
