using Common.Authentication;
using Common.Types;
using Identity.API.Application.Commands.CreateDeliveryAddress;
using Identity.API.Application.Commands.RemoveDeliveryAddress;
using Identity.API.Application.Commands.UpdateDeliveryAddress;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/address/")]
    public class DeliveryAddressController : BaseController
    {
        private readonly IMediator _mediator;

        public DeliveryAddressController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("")]
        [JwtAuthorize]
        public async Task<Guid> Create(CreateDeliveryAddressCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("")]
        [JwtAuthorize]
        public async Task<IActionResult> Update(UpdateDeliveryAddressCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("")]
        [JwtAuthorize]
        public async Task<IActionResult> Remove(RemoveDeliveryAddressCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}
