using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreateOfferDraftOne;
using Offers.API.Application.Commands.RemoveOfferDraft;
using Offers.API.Application.Commands.UpdateOfferDraftOne;
using Offers.API.Application.Commands.UpdateOfferDraftTwo;
using System;
using System.Threading.Tasks;

namespace Offers.API.Controllers
{
    [ApiController]
    [Route("/api/draft")]
    public class DraftController : BaseController
    {
        private readonly IMediator _mediator;

        public DraftController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("")]
        [JwtAuthorize]
        public async Task<IActionResult> CreateDraftOne([FromForm] CreateOfferDraftOneCommand command)
        {
            var offerId = await _mediator.Send(command);
            return Ok(new { OfferId = offerId });
        }

        [HttpPut("{offerId}/stage/1")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateDraftOne(string offerId, [FromForm] UpdateOfferDraftOneCommand command)
        {
            command.OfferId = offerId;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{offerId}/stage/2")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateDraftTwo(string offerId, [FromForm] UpdateOfferDraftTwoCommand command)
        {
            command.OfferId = offerId;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{offerId}")]
        [JwtAuthorize]
        public async Task<IActionResult> RemoveDraft([FromRoute] RemoveOfferDraftCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
