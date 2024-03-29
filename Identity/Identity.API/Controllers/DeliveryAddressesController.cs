﻿using Common.Utilities.Authentication;
using Common.Utilities.Types;
using Identity.API.Application.Commands.CreateDeliveryAddress;
using Identity.API.Application.Commands.RemoveDeliveryAddress;
using Identity.API.Application.Commands.UpdateDeliveryAddress;
using Identity.API.Application.Queries.GetDeliveryAddresses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/delivery-addresses/")]
    public class DeliveryAddressesController : BaseController
    {
        private readonly IMediator _mediator;

        public DeliveryAddressesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        [JwtAuthorize]
        public async Task<GetDeliveryAddressesResponse> GetAll()
        {
            var query = new GetDeliveryAddressesQuery();
            return await _mediator.Send(query);
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
