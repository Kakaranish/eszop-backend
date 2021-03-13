using Common.Authentication;
using Common.Types;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Commands.ReadAll;
using System;
using System.Threading.Tasks;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class NotificationsController : BaseController
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("all/read")]
        [JwtAuthorize]
        public async Task<IActionResult> ReadAll()
        {
            var command = new ReadAllCommand();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{notificationId}")]
        [JwtAuthorize]
        public async Task<IActionResult> DeleteById([FromRoute] string notificationId)
        {
            return Ok();
        }

        [HttpDelete("all")]
        [JwtAuthorize]
        public async Task<IActionResult> DeleteAll()
        {
            return Ok();
        }
    }
}
