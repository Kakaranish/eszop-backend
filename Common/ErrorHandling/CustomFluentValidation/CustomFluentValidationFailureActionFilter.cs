using System.Linq;
using System.Threading.Tasks;
using Common.ErrorHandling.ActionResults;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.ErrorHandling.CustomFluentValidation
{
    public class CustomFluentValidationFailureActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
                return;
            }

            if (context.ActionArguments.Count == 0)
            {
                context.Result = new ErrorActionResult("Invalid request");
                return;
            }

            var errorsInModelState = context
                .ModelState
                .Where(kvp => kvp.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(error => error.ErrorMessage));

            context.Result = new ValidationFailureActionResult
            {
                PropertyErrorsList = errorsInModelState.Select(kvp => new PropertyErrors
                {
                    Property = kvp.Key,
                    Errors = kvp.Value.Select(errMsg => new Error(errMsg))
                })
            };
        }
    }
}
