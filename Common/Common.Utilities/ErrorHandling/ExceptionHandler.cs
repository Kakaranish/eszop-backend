using System;
using System.Diagnostics;
using Common.Utilities.ErrorHandling.ActionResults;
using Common.Utilities.EventBus;
using Common.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.Utilities.ErrorHandling
{
    public class ExceptionHandler<TDomainException> where TDomainException : Exception
    {
        private readonly ILoggerFactory _loggerFactory;

        public ExceptionHandler(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public IActionResult HandleException(Exception exception)
        {
            if (exception is ValidationException validationException)
                return new ValidationFailureActionResult { PropertyErrorsList = validationException.PropertiesErrors };
            if (exception is TDomainException ordersDomainException)
                return new ErrorActionResult(ordersDomainException.Message);
            if (exception is UnauthorizedException) return new UnauthorizedResult();
            if (exception is ForbiddenException) return new StatusCodeResult(403);
            if (exception is NotFoundException notFoundException) return new ErrorActionResult(notFoundException.Message, 400); // It's not possible to return 404 :)
            if (exception is IntegrationEventException integrationEventException)
            {
                var trace = new StackTrace(integrationEventException, true);
                var throwingType = trace.GetFrame(0).GetMethod().ReflectedType;
                var logger = _loggerFactory.CreateLogger(throwingType);
                logger.LogError(integrationEventException.Message);
            }

            return new ErrorActionResult("Internal error", 500);
        }
    }
}
