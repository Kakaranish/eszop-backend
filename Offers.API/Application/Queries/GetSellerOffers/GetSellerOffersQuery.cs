using Common.Extensions;
using Common.Types;
using FluentValidation;
using MediatR;
using Offers.API.Application.Dto;
using Offers.API.Application.Types;

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
