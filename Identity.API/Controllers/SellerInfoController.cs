using Common.Authentication;
using Common.Dto;
using Common.Types;
using Identity.API.Application.Commands.CreateOrUpdateSellerInfo;
using Identity.API.Application.Queries.GetBankAccountNumber;
using Identity.API.Application.Queries.GetPublicSellerInfo;
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

        [HttpGet("{sellerId}")]
        public async Task<PublicSellerInfoDto> GetPublicSellerInfo([FromRoute] GetPublicSellerInfoQuery request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("{sellerId}/bank-account-number")]
        [JwtAuthorize]
        public async Task<string> GetPublicSellerInfo([FromRoute] GetBankAccountNumberQuery request)
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
