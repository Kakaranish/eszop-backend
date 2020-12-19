using Common.Authentication;
using Common.Types;
using Identity.API.DataAccess.Repositories;
using Identity.API.Dto;
using Identity.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class PasswordController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly IPasswordHasher _passwordHasher;
        private readonly HttpContext _httpContext;

        public PasswordController(IUserRepository userRepository, IDistributedCache distributedCache,
            IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        [HttpPost("generate/reset-token")]
        public async Task<IActionResult> GenerateResetToken([FromBody] string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null) return ErrorResponse("There is no user with such email");

            var resetToken = RandomStringGenerator.Generate(50);
            var userId = Encoding.UTF8.GetBytes(user.Id.ToString());
            await _distributedCache.SetAsync(resetToken, userId, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });

            return Ok(resetToken);
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset(ResetPasswordDto resetPasswordDto)
        {
            var userIdAsBytes = await _distributedCache.GetAsync(resetPasswordDto.ResetToken);
            if (userIdAsBytes == null) return ErrorResponse("Invalid reset token");

            var userIdAsStr = Encoding.UTF8.GetString(userIdAsBytes);
            if (!Guid.TryParse(userIdAsStr, out var userId)) return ErrorResponse("Invalid userId in reset token");

            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null) return ErrorResponse("No such user");

            var password = _passwordHasher.Hash(resetPasswordDto.NewPassword);
            user.SetPassword(password);
            await _userRepository.UpdateAsync(user);

            await _distributedCache.RemoveAsync(resetPasswordDto.ResetToken);

            return Ok();
        }

        [JwtAuthorize]
        [HttpPost("change")]
        public async Task<IActionResult> Change(ChangePasswordDto changePasswordDto)
        {
            var userIdStr = _httpContext.User.Claims
                .FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            if (!Guid.TryParse(userIdStr, out var userId)) return ErrorResponse("No UserId claim in access token");

            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null) return ErrorResponse($"No user with id '{userIdStr}'");

            var isOldPasswordValid = _passwordHasher.Verify(changePasswordDto.OldPassword, user.HashedPassword);
            if (!isOldPasswordValid) return ErrorResponse("Invalid old password");

            var newPasswordHashed = _passwordHasher.Hash(changePasswordDto.NewPassword);
            user.SetPassword(newPasswordHashed);
            await _userRepository.UpdateAsync(user);

            return Ok();
        }
    }
}
