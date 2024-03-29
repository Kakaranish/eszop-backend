﻿using Common.Utilities.Authentication;
using Common.Utilities.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreatePredefinedDeliveryMethod;
using Offers.API.Application.Commands.RemovePredefinedDeliveryMethod;
using Offers.API.Application.Commands.UpdatePredefinedDeliveryMethod;
using Offers.API.Application.Queries.GetAllPredefinedDeliveryMethods;
using Offers.API.Application.Queries.GetPredefinedDeliveryMethodById;
using Offers.Infrastructure.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.Controllers
{
    [ApiController]
    [Route("/api/delivery-methods/")]
    public class PredefinedDeliveryMethodController : BaseController
    {
        private readonly IMediator _mediator;

        public PredefinedDeliveryMethodController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("")]
        public async Task<IList<PredefinedDeliveryMethodDto>> GetAll()
        {
            var query = new GetAllPredefinedDeliveryMethodsQuery();
            return await _mediator.Send(query);
        }

        [HttpGet("{deliveryMethodId}")]
        public async Task<PredefinedDeliveryMethodDto> GetById([FromRoute] GetPredefinedDeliveryMethodByIdQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("")]
        [JwtAuthorize("SuperAdmin")]
        public async Task<Guid> Create(CreatePredefinedDeliveryMethodCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{deliveryMethodId}")]
        [JwtAuthorize("SuperAdmin")]
        public async Task<IActionResult> Update([FromRoute] string deliveryMethodId,
            UpdatePredefinedDeliveryMethodCommand command)
        {
            command.DeliveryMethodId = deliveryMethodId;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{deliveryMethodId}")]
        [JwtAuthorize("SuperAdmin")]
        public async Task<IActionResult> Remove([FromRoute] RemovePredefinedDeliveryMethodCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
