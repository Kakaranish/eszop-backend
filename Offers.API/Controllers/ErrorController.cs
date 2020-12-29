using Common.ErrorHandling;
using Common.Types;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Offers.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseController
    {
        [Route("error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;

            if (exception is ValidationException validationException)
            {
                return ValidationFailureResponse(validationException.PropertiesErrors);
            }
            
            return ErrorResponse("Internal error");
        }
    }
}
