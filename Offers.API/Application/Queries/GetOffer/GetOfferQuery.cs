using Common.Extensions;
using FluentValidation;
using MediatR;
using Offers.API.Application.Dto;

namespace Offers.API.Application.Queries.GetOffer
{
    public class GetOfferQuery : IRequest<OfferDto>
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
