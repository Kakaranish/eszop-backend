using System.Collections.Generic;
using MediatR;
using Offers.API.Domain;

namespace Offers.API.Application.Queries.GetAllOffers
{
    public class GetAllOffersQuery : IRequest<IList<Offer>>
    {
    }
}
