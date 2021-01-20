using MediatR;
using Offers.API.Application.Dto;
using System.Collections.Generic;

namespace Offers.API.Application.Queries.GetUserOffers
{
    public class GetUserOffersQuery : IRequest<IList<OfferDto>>
    {
    }
}
