using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreateOffer;
using Offers.API.Application.Commands.EndOffer;
using Offers.API.Application.Commands.UpdateOffer;
using Offers.API.Application.Queries.GetOffer;
using Offers.API.Domain;
using System;
using System.Threading.Tasks;

namespace Offers.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class OffersController : BaseController
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{offerId}")]
        public async Task<Offer> GetById(string offerId)
        {
            var query = new GetOfferQuery { OfferId = offerId };
            return await _mediator.Send(query);
        }

        [HttpPost("")]
        [JwtAuthorize]
        public async Task<IActionResult> Create(CreateOfferCommand command)
        {
            var offerId = await _mediator.Send(command);
            return Ok(new { OfferId = offerId });
        }

        [HttpPut("")]
        [JwtAuthorize]
        public async Task<IActionResult> Update(UpdateOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("end")]
        [JwtAuthorize]
        public async Task<IActionResult> End(EndOfferCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
