using Common.Authentication;
using Common.Types;
using Identity.API.Application.Commands.LockUser;
using Identity.API.Application.Commands.UnlockUser;
using Identity.API.Application.Dto;
using Identity.API.Application.Queries.GetUser;
using Identity.API.Application.Queries.GetUsers;
using Identity.API.Application.Types;
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
        [HttpGet("users")]
        public async Task<Pagination<UserPreviewDto>> GetUsers([FromQuery] UserFilter filter)
        {
            var query = new GetUsersQuery { UserFilter = filter };
            return await _mediator.Send(query);
        }

        [JwtAuthorize("Admin")]
        [HttpGet("users/{userId}")]
        public async Task<UserPreviewDto> GetUserById([FromRoute] GetUserQuery query)
        {
            return await _mediator.Send(query);
        }

        [JwtAuthorize("Admin")]
        [HttpPost("users/{userId}/lock")]
        public async Task<IActionResult> LockUser([FromRoute] LockUserCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [JwtAuthorize("Admin")]
        [HttpPost("users/{userId}/unlock")]
        public async Task<IActionResult> UnlockUser([FromRoute] UnlockUserCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
