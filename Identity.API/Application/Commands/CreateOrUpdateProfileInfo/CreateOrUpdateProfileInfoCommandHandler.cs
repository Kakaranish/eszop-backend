using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using Identity.API.Domain.CommonValidators;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.CreateOrUpdateProfileInfo
{
    public class CreateOrUpdateProfileInfoCommandHandler : IRequestHandler<CreateOrUpdateProfileInfoCommand>
    {
        private readonly IProfileInfoRepository _profileInfoRepository;
        private readonly HttpContext _httpContext;

        public CreateOrUpdateProfileInfoCommandHandler(IHttpContextAccessor httpContextAccessor,
            IProfileInfoRepository profileInfoRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _profileInfoRepository = profileInfoRepository ?? throw new ArgumentNullException(nameof(profileInfoRepository));
        }

        public async Task<Unit> Handle(CreateOrUpdateProfileInfoCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var profileInfo = await _profileInfoRepository.GetByUserIdAsync(userId);
            var profileInfoExits = profileInfo != null;

            var dateOfBirth = request.DateOfBirth != null
                ? DateTime.ParseExact(request.DateOfBirth, DateOfBirthStrValidator.DateOfBirthFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None)
                : (DateTime?)null;

            if (!profileInfoExits)
            {
                if (AnyPropertyHasNullValue(request))
                    throw new IdentityDomainException("At least one property in request is null");

                profileInfo = new ProfileInfo(userId, request.FirstName, request.LastName, dateOfBirth.Value,
                    request.PhoneNumber);
                _profileInfoRepository.Add(profileInfo);
            }
            else
            {
                if (request.FirstName != null) profileInfo.SetFirstName(request.FirstName);
                if (request.LastName != null) profileInfo.SetLastName(request.LastName);
                if (dateOfBirth != null) profileInfo.SetDateOfBirth(dateOfBirth.Value);
                if (request.PhoneNumber != null) profileInfo.SetPhoneNumber(request.PhoneNumber);
                _profileInfoRepository.Update(profileInfo);
            }

            await _profileInfoRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }

        private static bool AnyPropertyHasNullValue(CreateOrUpdateProfileInfoCommand request)
        {
            foreach (var propertyInfo in request.GetType().GetProperties())
            {
                var value = (string)propertyInfo.GetValue(request);
                if (value == null) return true;
            }

            return false;
        }
    }
}
