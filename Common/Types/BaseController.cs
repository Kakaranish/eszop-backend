using System.Collections.Generic;
using Common.ErrorHandling;
using Common.ErrorHandling.ActionResults;
using Microsoft.AspNetCore.Mvc;

namespace Common.Types
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
