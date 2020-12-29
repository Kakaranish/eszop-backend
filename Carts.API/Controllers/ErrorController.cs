using System;
using Carts.API.Domain;
using Common.ErrorHandling;
using Common.Types;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Carts.API.Controllers
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
                return ValidationFailureResponse(validationException.PropertiesErrors);
            if (exception is CartsDomainException cartDomainException)
                return ErrorResponse(cartDomainException.Message);

            _logger.LogError(exception.ToString());
            return ErrorResponse("Internal error");
        }
    }
}
