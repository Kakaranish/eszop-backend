using Common.Utilities.Authentication;
using Common.Utilities.Types;
using Identity.API.Application.Commands.ChangePassword;
using Identity.API.Application.Commands.GenerateResetToken;
using Identity.API.Application.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class PasswordController : BaseController
    {
        private readonly IMediator _mediator;

        public PasswordController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("generate/reset-token")]
        public async Task<IActionResult> GenerateResetToken(GenerateResetTokenCommand request)
        {
            var token = await _mediator.Send(request);
            return Ok(new { ResetToken = token });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> Reset(ResetPasswordCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [JwtAuthorize]
        [HttpPost("change")]
        public async Task<IActionResult> Change(ChangePasswordCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
