using Common.Exceptions;
using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.LockUser
{
    public class LockUserCommandHandler : IRequestHandler<LockUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public LockUserCommandHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Unit> Handle(LockUserCommand request, CancellationToken cancellationToken)
        {
            var requesterId = _httpContext.User.Claims.ToTokenPayload().Id;
            var userId = Guid.Parse(request.UserId);
            var lockedUntil = DateTime.ParseExact(request.LockUntil, LockUserCommand.LockUntilFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User");
            if (user.Id == requesterId) throw new IdentityDomainException("User cannot lock himself/herself");
            if (user.IsLocked) throw new IdentityDomainException("User is already locked");

            user.SetLockedUntil(lockedUntil);

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
