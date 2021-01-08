using Common.Authentication;
using Common.Types;
using Identity.API.Application.Commands.CreateOrUpdateProfileInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class ProfileInfoController : BaseController
    {
        private readonly IMediator _mediator;

        public ProfileInfoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPut("")]
        [JwtAuthorize]
        public async Task<IActionResult> CreateOrUpdateProfileInfo(CreateOrUpdateProfileInfoCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
