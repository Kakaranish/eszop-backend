using Common.Authentication;
using Common.Types;
using Identity.API.Application.Commands.LockUser;
using Identity.API.Application.Commands.UnlockUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AdminController : BaseController
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [JwtAuthorize("Admin")]
        [HttpPost("lock")]
        public async Task<IActionResult> LockUser(LockUserCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [JwtAuthorize("Admin")]
        [HttpPost("unlock")]
        public async Task<IActionResult> UnlockUser(UnlockUserCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
