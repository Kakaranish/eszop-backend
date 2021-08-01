using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Common.Utilities.ErrorHandling
{
    public static class Extensions
    {
        public static IEnumerable<PropertyErrors> ToPropertiesErrors(this IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures
                .GroupBy(failure => failure.PropertyName)
                .Select(group => new PropertyErrors
                {
                    Property = group.Key,
                    Errors = group.Select(failure => new Error
                    {
                        Message = failure.ErrorMessage,
                        Code = failure.ErrorCode
                    })
                });
        }
    }
}
