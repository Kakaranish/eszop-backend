using MediatR;
using Offers.API.Application.Dto;
using Offers.API.DataAccess.Repositories;
using Offers.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Queries.GetAllPredefinedDeliveryMethods
{
    public class GetAllPredefinedDeliveryMethodsQueryHandler :
        IRequestHandler<GetAllPredefinedDeliveryMethodsQuery, IList<PredefinedDeliveryMethodDto>>
    {
        private readonly IPredefinedDeliveryMethodRepository _predefinedDeliveryMethodRepository;

        public GetAllPredefinedDeliveryMethodsQueryHandler(
            IPredefinedDeliveryMethodRepository predefinedDeliveryMethodRepository)
        {
            _predefinedDeliveryMethodRepository = predefinedDeliveryMethodRepository ??
                                                  throw new ArgumentNullException(nameof(predefinedDeliveryMethodRepository));
        }

        public async Task<IList<PredefinedDeliveryMethodDto>> Handle(
            GetAllPredefinedDeliveryMethodsQuery request, CancellationToken cancellationToken)
        {
            var deliveryMethods = await _predefinedDeliveryMethodRepository.GetAll();

            return deliveryMethods.Select(deliveryMethod => deliveryMethod.ToDto()).ToList();
        }
    }
}
