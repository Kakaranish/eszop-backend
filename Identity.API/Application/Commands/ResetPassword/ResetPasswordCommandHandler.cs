using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using Identity.API.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ResetPasswordCommandHandler(IDistributedCache distributedCache,
            IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var userIdAsBytes = await _distributedCache.GetAsync(request.ResetToken, cancellationToken);
            if (userIdAsBytes == null) throw new IdentityDomainException("Invalid reset token");

            var userIdAsStr = Encoding.UTF8.GetString(userIdAsBytes);
            if (!Guid.TryParse(userIdAsStr, out var userId)) throw new IdentityDomainException("Invalid reset token");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.Email != request.Email) throw new IdentityDomainException("Invalid reset token");

            var password = _passwordHasher.Hash(request.NewPassword);
            user.SetPassword(password);
            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            await _distributedCache.RemoveAsync(request.ResetToken, cancellationToken);

            return await Unit.Task;
        }
    }
}
