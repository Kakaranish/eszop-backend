using Common.Extensions;
using Identity.API.Application.Dto;
using Identity.API.DataAccess.Repositories;
using Identity.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.GetDeliveryAddresses
{
    public class GetDeliveryAddressesQueryHandler : IRequestHandler<GetDeliveryAddressesQuery, IList<DeliveryAddressDto>>
    {
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;
        private readonly HttpContext _httpContext;

        public GetDeliveryAddressesQueryHandler(IDeliveryAddressRepository deliveryAddressRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _deliveryAddressRepository = deliveryAddressRepository ?? throw new ArgumentNullException(nameof(deliveryAddressRepository));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        }

        public async Task<IList<DeliveryAddressDto>> Handle(GetDeliveryAddressesQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var deliveryAddresses = await _deliveryAddressRepository.GetByUserId(userId);

            return deliveryAddresses.Select(x => x.ToDto()).ToList();
        }
    }
}
