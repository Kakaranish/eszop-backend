using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace Common.Utilities.ErrorHandling
{
    public class ValidationException : Exception
    {
        public IEnumerable<PropertyErrors> PropertiesErrors { get; }

        public ValidationException(IEnumerable<ValidationFailure> validationFailures)
        {
            PropertiesErrors = validationFailures?.ToPropertiesErrors() ?? throw new ArgumentNullException(nameof(validationFailures));
        }

        public ValidationException(IEnumerable<PropertyErrors> propertiesErrors)
        {
            PropertiesErrors = propertiesErrors ?? throw new ArgumentNullException(nameof(propertiesErrors));
        }
    }
}
