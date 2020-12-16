using Common.Types.ErrorHandling.ActionResults;
using Microsoft.AspNetCore.Mvc;

namespace Common.Types
{
    public class BaseController : Controller
    {
        public static IActionResult ErrorResponse(string message)
        {
            return new ErrorActionResult(message);
        }
    }
}
