using MediatR;
using Offers.API.Application.Dto;
using System.Collections.Generic;

namespace Offers.API.Application.Queries.GetAllPredefinedDeliveryMethods
{
    public class GetAllPredefinedDeliveryMethodsQuery : IRequest<IList<PredefinedDeliveryMethodDto>>
    {
    }
}
