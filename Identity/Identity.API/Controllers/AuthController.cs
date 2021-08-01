using Common.Utilities.Types;
using Identity.API.Application.Commands.SignIn;
using Identity.API.Application.Commands.SignOut;
using Identity.API.Application.Commands.SignUp;
using Identity.API.Application.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("sign-in")]
        public async Task<TokenResponse> SignIn(SignInCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOut(SignOutCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("sign-up")]
        public async Task<TokenResponse> SignUp(SignUpCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}
