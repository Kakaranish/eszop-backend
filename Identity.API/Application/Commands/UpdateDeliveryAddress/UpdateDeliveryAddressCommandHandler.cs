using Common.Utilities.Exceptions;
using Common.Utilities.Extensions;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.UpdateDeliveryAddress
{
    public class UpdateDeliveryAddressCommandHandler : IRequestHandler<UpdateDeliveryAddressCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IUserRepository _userRepository;

        public UpdateDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Unit> Handle(UpdateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var deliveryAddressId = Guid.Parse(request.DeliveryAddressId);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User");

            var deliveryAddress = user.DeliveryAddresses.FirstOrDefault(x => x.Id == deliveryAddressId);
            if (deliveryAddress == null) throw new NotFoundException("Delivery address");

            if (request.FirstName != null) deliveryAddress.SetFirstName(request.FirstName);
            if (request.LastName != null) deliveryAddress.SetLastName(request.LastName);
            if (request.PhoneNumber != null) deliveryAddress.SetPhoneNumber(request.PhoneNumber);
            if (request.Country != null) deliveryAddress.SetCountry(request.Country);
            if (request.City != null) deliveryAddress.SetCity(request.City);
            if (request.ZipCode != null) deliveryAddress.SetZipCode(request.ZipCode);
            if (request.Street != null) deliveryAddress.SetStreet(request.Street);


            if (request.IsPrimary)
                user.SetPrimaryDeliveryAddress(deliveryAddressId);
            else if (!request.IsPrimary && user.PrimaryDeliveryAddressId == deliveryAddressId)
                user.UnsetPrimaryDeliveryAddress();

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
