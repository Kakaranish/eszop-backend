using Common.Extensions;
using Identity.API.Application.Dto;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
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
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public GetDeliveryAddressesQueryHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IList<DeliveryAddressDto>> Handle(GetDeliveryAddressesQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new IdentityDomainException("There is no such user");

            return user.DeliveryAddresses?.Select(x => x.ToDto()).ToList()
                                    ?? new List<DeliveryAddressDto>();
        }
    }
}
