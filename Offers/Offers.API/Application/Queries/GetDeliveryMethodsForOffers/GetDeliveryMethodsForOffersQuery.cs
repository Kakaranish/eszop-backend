using Common.Dto;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Offers.API.Application.Queries.GetDeliveryMethodsForOffers
{
    public class GetDeliveryMethodsForOffersQuery : IRequest<IList<DeliveryMethodDto>>
    {
        [FromQuery(Name = "ids")] public IList<string> OfferIds { get; init; }
    }

    public class GetDeliveryMethodsForOffersQueryValidator : AbstractValidator<GetDeliveryMethodsForOffersQuery>
    {
        public GetDeliveryMethodsForOffersQueryValidator()
        {
            RuleFor(x => x.OfferIds)
                .NotNull()
                .Must(x => x.Count > 0)
                .WithMessage("Must contain at least 1 id")
                .Must(offerIds => offerIds.All(offerIdStr =>
                {
                    var parsable = Guid.TryParse(offerIdStr, out var offerId);
                    return parsable && offerId != Guid.Empty;
                }))
                .WithMessage("Each id must be not empty guid");
        }
    }
}
