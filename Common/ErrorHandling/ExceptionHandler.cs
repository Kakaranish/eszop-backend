using System;
using System.Diagnostics;
using Common.ErrorHandling.ActionResults;
using Common.EventBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.ErrorHandling
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
            if (exception is IntegrationEventException integrationEventException)
            {
                var trace = new StackTrace(integrationEventException, true);
                var throwingType = trace.GetFrame(0).GetMethod().ReflectedType;
                var logger = _loggerFactory.CreateLogger(throwingType);
                logger.LogError(integrationEventException.Message);
            }

            return new ErrorActionResult("Internal error");
        }
    }
}
