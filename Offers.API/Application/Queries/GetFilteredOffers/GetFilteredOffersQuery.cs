using Common.Types;
using FluentValidation;
using MediatR;
using Offers.API.Application.Dto;
using System;

namespace Offers.API.Application.Queries.GetFilteredOffers
{
    public class GetFilteredOffersQuery : IRequest<Pagination<OfferDto>>
    {
        public int PageSize { get; init; } = 10;
        public int PageIndex { get; init; } = 1;
        public decimal? FromPrice { get; init; }
        public decimal? ToPrice { get; init; }
        public Guid? Category { get; init; }
    }

    public class GetFilteredOffersQueryValidator : AbstractValidator<GetFilteredOffersQuery>
    {
        public GetFilteredOffersQueryValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0);

            RuleFor(x => x.PageIndex)
                .GreaterThan(0);

            RuleFor(x => x.Category)
                .Must(x => x != Guid.Empty)
                .WithMessage("Cannot be empty guid");

            RuleFor(x => x.FromPrice)
                .GreaterThan(0)
                .When(x => x.FromPrice != null);

            RuleFor(x => x.ToPrice)
                .GreaterThan(0)
                .When(x => x.ToPrice != null);

            RuleFor(x => x.ToPrice)
                .GreaterThanOrEqualTo(x => x.FromPrice)
                .When(x => x.FromPrice != null && x.ToPrice != null);
        }
    }
}
