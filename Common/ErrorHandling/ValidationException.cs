using System;
using System.Collections.Generic;

namespace Common.ErrorHandling
{
    public class ValidationException : Exception
    {
        public IEnumerable<PropertyErrors> PropertiesErrors { get; }

        public ValidationException(IEnumerable<PropertyErrors> propertiesErrors)
        {
            PropertiesErrors = propertiesErrors ?? throw new ArgumentNullException(nameof(propertiesErrors));
        }
    }
}
