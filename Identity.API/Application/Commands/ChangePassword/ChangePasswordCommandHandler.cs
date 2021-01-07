using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using Identity.API.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly HttpContext _httpContext;

        public ChangePasswordCommandHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userIdStr = _httpContext.User.Claims
                .FirstOrDefault(claim => claim.Type == "UserId")?.Value;

            if (!Guid.TryParse(userIdStr, out var userId))
                throw new IdentityDomainException("No UserId claim in access token");

            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null) throw new IdentityDomainException($"No user with id '{userIdStr}'");

            var isOldPasswordValid = _passwordHasher.Verify(request.OldPassword, user.HashedPassword);
            if (!isOldPasswordValid) throw new IdentityDomainException("Invalid old password");

            var newPasswordHashed = _passwordHasher.Hash(request.NewPassword);
            user.SetPassword(newPasswordHashed);

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
