using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.ErrorHandling
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationFailures = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure != null)
                .ToList();

            if (validationFailures.Any())
            {
                _logger.LogWarning($"{typeof(TRequest)} failed");
                throw new ValidationException(validationFailures.ToPropertiesErrors());
            }

            _logger.LogInformation($"Processing {typeof(TRequest).Name} request");

            return next();
        }
    }
}
