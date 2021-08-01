using Common.Domain.Types;
using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;
using Offers.Domain.Repositories.Types;
using Offers.Infrastructure.Dto;

namespace Offers.API.Application.Queries.GetSellerOffers
{
    public class GetSellerOffersQuery : IRequest<Pagination<OfferListPreviewDto>>
    {
        public string SellerId { get; init; }
        public OfferFilter OfferFilter { get; init; }
    }

    public class GetSellerOffersQueryValidator : AbstractValidator<GetSellerOffersQuery>
    {
        public GetSellerOffersQueryValidator()
        {
            RuleFor(x => x.SellerId)
                .IsNotEmptyGuid();

            RuleFor(x => x.OfferFilter)
                .SetValidator(new OfferFilterValidator());
        }
    }
}
