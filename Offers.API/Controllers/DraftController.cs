using Common.Utilities.Authentication;
using Common.Utilities.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offers.API.Application.Commands.CreateOfferDraft;
using Offers.API.Application.Commands.PublishOffer;
using Offers.API.Application.Commands.RemoveOfferDraft;
using Offers.API.Application.Commands.UpdateOfferDraft;
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
        public async Task<Guid> CreateDraft([FromForm] CreateOfferDraftCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{offerId}")]
        [JwtAuthorize]
        public async Task<IActionResult> UpdateDraft(string offerId, [FromForm] UpdateOfferDraftCommand command)
        {
            command.OfferId = offerId;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("{offerId}/publish")]
        [JwtAuthorize]
        public async Task<IActionResult> Publish([FromRoute] PublishOfferCommand command)
        {
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
