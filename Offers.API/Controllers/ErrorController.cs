using Common.ErrorHandling;
using Common.Types;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Offers.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseController
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;

            if (exception is ValidationException validationException)
            {
                return ValidationFailureResponse(validationException.PropertiesErrors);
            }
            if (exception is IntegrationEventException integrationEventException)
            {
                _logger.LogCritical(integrationEventException.Message);
            }

            return ErrorResponse("Internal error");
        }
    }
}
