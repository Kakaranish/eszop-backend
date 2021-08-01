using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using Carts.Domain.Exceptions;
using Common.Utilities.ErrorHandling;
using Common.Utilities.Types;

namespace Carts.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseController
    {
        private readonly ExceptionHandler<CartsDomainException> _exceptionHandler;

        public ErrorController(ExceptionHandler<CartsDomainException> exceptionHandler)
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
