using Common.Authentication;
using Common.Types;
using Identity.API.Application.Commands.LockUser;
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
    }
}
