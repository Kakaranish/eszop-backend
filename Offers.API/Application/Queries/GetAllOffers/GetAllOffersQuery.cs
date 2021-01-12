using MediatR;
using Offers.API.Application.Dto;
using System.Collections.Generic;

namespace Offers.API.Application.Queries.GetAllOffers
{
    public class GetAllOffersQuery : IRequest<IList<OfferDto>>
    {
    }
}
