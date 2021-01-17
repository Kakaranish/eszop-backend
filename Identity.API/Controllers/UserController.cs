using Common.Authentication;
using Identity.API.Application.Dto;
using Identity.API.Application.Queries.GetMe;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("me")]
        [JwtAuthorize]
        public async Task<MeDto> GetMe()
        {
            var query = new GetMeQuery();
            return await _mediator.Send(query);
        }
    }
}
