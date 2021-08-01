using Common.Utilities.Exceptions;
using Common.Utilities.Extensions;
using Identity.Domain.Exceptions;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
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

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new NotFoundException("User");
            if (user.Id == requesterId) throw new IdentityDomainException("User cannot lock himself/herself");

            user.SetLocked();

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
