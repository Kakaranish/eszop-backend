using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressCommandHandler : IRequestHandler<UpdateDeliveryAddressCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;

        public UpdateDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor,
            IDeliveryAddressRepository deliveryAddressRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _deliveryAddressRepository = deliveryAddressRepository ??
                                         throw new ArgumentNullException(nameof(deliveryAddressRepository));
        }
        public async Task<Unit> Handle(UpdateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var deliveryAddressId = Guid.Parse(request.DeliveryAddressId);
            var deliveryAddress = await _deliveryAddressRepository.GetById(deliveryAddressId);

            if (deliveryAddress == null || deliveryAddress.UserId != userId)
                throw new IdentityDomainException($"There is no {deliveryAddressId} delivery address");

            if (request.FirstName != null) deliveryAddress.SetFirstName(request.FirstName);
            if (request.LastName != null) deliveryAddress.SetLastName(request.LastName);
            if (request.PhoneNumber != null) deliveryAddress.SetPhoneNumber(request.PhoneNumber);
            if (request.Country != null) deliveryAddress.SetCountry(request.Country);
            if (request.City != null) deliveryAddress.SetCity(request.City);
            if (request.ZipCode != null) deliveryAddress.SetZipCode(request.ZipCode);
            if (request.Street != null) deliveryAddress.SetStreet(request.Street);

            _deliveryAddressRepository.Update(deliveryAddress);
            await _deliveryAddressRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
