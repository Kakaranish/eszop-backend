using MediatR;
using Offers.API.Domain;
using System.Collections.Generic;

namespace Offers.API.Application.Queries.GetAllOffers
{
    public class GetAllOffersQuery : IRequest<IList<Offer>>
    {
    }
}
