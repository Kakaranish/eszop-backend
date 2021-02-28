using Common.Exceptions;
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
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public CreateDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Guid> Handle(CreateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException();

            var deliveryAddress = new DeliveryAddress(request.FirstName, request.LastName,
                request.PhoneNumber, request.Country, request.City, request.ZipCode, request.Street);
            user.AddDeliveryAddress(deliveryAddress);
            if (request.IsPrimary) user.SetPrimaryDeliveryAddress(deliveryAddress.Id);

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return deliveryAddress.Id;
        }
    }
}
