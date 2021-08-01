using Common.Extensions;
using FluentValidation;
using MediatR;
using Offers.Infrastructure.Dto;

namespace Offers.API.Application.Queries.GetOffer
{
    public class GetOfferQuery : IRequest<OfferViewDto>
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
