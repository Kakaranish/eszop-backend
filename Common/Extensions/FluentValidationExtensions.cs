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

        public static IRuleBuilderOptions<T, string> IsNotEmptyGuid<T>(this IRuleBuilderOptions<T, string> ruleBuilderOptions)
        {
            ruleBuilderOptions
                .Must(id => Guid.TryParse(id, out var parsedId) && parsedId != Guid.Empty)
                .WithMessage("Invalid or empty guid");

            return ruleBuilderOptions;
        }

        public static IRuleBuilderOptions<T, string> IsNotEmptyGuid<T>(this IRuleBuilderInitial<T, string> ruleBuilderInitial)
        {
            var ruleBuilderOptions = ruleBuilderInitial
                .Must(id => Guid.TryParse(id, out var parsedId) && parsedId != Guid.Empty)
                .WithMessage("Invalid or empty guid");

            return ruleBuilderOptions;
        }

        public static IRuleBuilderOptions<T, Guid> IsNotEmptyGuid<T>(this IRuleBuilderOptions<T, Guid> ruleBuilderOptions)
        {
            ruleBuilderOptions
                .Must(id => id != Guid.Empty)
                .WithMessage($"Cannot be {Guid.Empty}");

            return ruleBuilderOptions;
        }

        public static IRuleBuilderOptions<T, Guid> IsNotEmptyGuid<T>(this IRuleBuilderInitial<T, Guid> ruleBuilderInitial)
        {
            var ruleBuilderOptions = ruleBuilderInitial
                .Must(id => id != Guid.Empty)
                .WithMessage($"Cannot be {Guid.Empty}");

            return ruleBuilderOptions;
        }
    }
}
