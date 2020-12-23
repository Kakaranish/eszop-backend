using FluentValidation;
using System;

namespace Common.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilderOptions<T, string> ruleBuilderOptions)
        {
            ruleBuilderOptions
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("invalid guid");


            return ruleBuilderOptions;
        }

        public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilderInitial<T, string> ruleBuilderInitial)
        {
            var ruleBuilderOptions = ruleBuilderInitial
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("invalid guid");

            return ruleBuilderOptions;
        }
    }
}
