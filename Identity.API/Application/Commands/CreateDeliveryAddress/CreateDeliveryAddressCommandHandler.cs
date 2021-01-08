using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.CreateDeliveryAddress
{
    public class CreateDeliveryAddressCommandHandler : IRequestHandler<CreateDeliveryAddressCommand, Guid>
    {
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;
        private readonly HttpContext _httpContext;

        public CreateDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor,
            IDeliveryAddressRepository deliveryAddressRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _deliveryAddressRepository = deliveryAddressRepository ??
                                         throw new ArgumentNullException(nameof(deliveryAddressRepository));
        }

        public async Task<Guid> Handle(CreateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var deliveryAddress = new DeliveryAddress(userId, request.FirstName, request.LastName,
                request.PhoneNumber, request.Country, request.City, request.ZipCode, request.Street);

            _deliveryAddressRepository.Add(deliveryAddress);
            await _deliveryAddressRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return deliveryAddress.Id;
        }
    }
}
