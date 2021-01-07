using Common.ErrorHandling;
using Common.Types;
using Identity.API.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Identity.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseController
    {
        private readonly ExceptionHandler<IdentityDomainException> _exceptionHandler;

        public ErrorController(ExceptionHandler<IdentityDomainException> exceptionHandler)
        {
            _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
        }

        [Route("error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;

            return _exceptionHandler.HandleException(exception);
        }
    }
}
