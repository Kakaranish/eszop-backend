using Common.Utilities.Exceptions;
using Identity.API.Services;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.GenerateResetToken
{
    public class GenerateResetTokenCommandHandler : IRequestHandler<GenerateResetTokenCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _distributedCache;

        public GenerateResetTokenCommandHandler(IUserRepository userRepository,
            IDistributedCache distributedCache)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task<string> Handle(GenerateResetTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) throw new NotFoundException("User");

            if (user.Role.IsAdminRole()) throw new ForbiddenException();

            var resetToken = RandomStringGenerator.Generate(50);
            var userId = Encoding.UTF8.GetBytes(user.Id.ToString());
            await _distributedCache.SetAsync(resetToken, userId, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }, cancellationToken);

            return resetToken;
        }
    }
}
