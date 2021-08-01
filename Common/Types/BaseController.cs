using System.Collections.Generic;
using Common.Utilities.ErrorHandling;
using Common.Utilities.ErrorHandling.ActionResults;
using Microsoft.AspNetCore.Mvc;

namespace Common.Utilities.Types
{
    public class BaseController : Controller
    {
        public static IActionResult ErrorResponse(string message)
        {
            return new ErrorActionResult(message);
        }

        public static IActionResult ValidationFailureResponse(IEnumerable<PropertyErrors> propertiesErrors)
        {
            return new ValidationFailureActionResult
            {
                PropertyErrorsList = propertiesErrors
            };
        }
    }
}
