using Common.Authentication;
using Common.Types;
using Identity.API.Application.Commands.CreateOrUpdateProfileInfo;
using Identity.API.Application.Dto;
using Identity.API.Application.Queries.GetProfileInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/profile-info")]
    public class ProfileInfoController : BaseController
    {
        private readonly IMediator _mediator;

        public ProfileInfoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        [JwtAuthorize]
        public async Task<ProfileInfoDto> Get()
        {
            var query = new GetProfileInfoQuery();
            return await _mediator.Send(query);
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
