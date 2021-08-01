using MediatR;
using System.Collections.Generic;
using Offers.Infrastructure.Dto;

namespace Offers.API.Application.Queries.GetAllPredefinedDeliveryMethods
{
    public class GetAllPredefinedDeliveryMethodsQuery : IRequest<IList<PredefinedDeliveryMethodDto>>
    {
    }
}
