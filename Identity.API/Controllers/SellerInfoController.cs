using Common.Utilities.Authentication;
using Common.Utilities.Types;
using Identity.API.Application.Commands.CreateOrUpdateSellerInfo;
using Identity.API.Application.Dto;
using Identity.API.Application.Queries.CanSell;
using Identity.API.Application.Queries.GetPublicSellerInfo;
using Identity.API.Application.Queries.GetSellerMe;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/seller")]
    public class SellerInfoController : BaseController
    {
        private readonly IMediator _mediator;

        public SellerInfoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("can-sell")]
        [JwtAuthorize]
        public async Task<bool> GetCanSell()
        {
            var query = new CanSellQuery();
            return await _mediator.Send(query);
        }

        [HttpGet("me")]
        [JwtAuthorize]
        public async Task<SellerInfoDto> GetSellerMe()
        {
            var query = new GetSellerMeQuery();
            return await _mediator.Send(query);
        }

        [HttpGet("{sellerId}")]
        public async Task<PublicSellerInfoDto> GetPublicSellerInfo([FromRoute] GetPublicSellerInfoQuery request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("")]
        [JwtAuthorize]
        public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateSellerInfoCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
