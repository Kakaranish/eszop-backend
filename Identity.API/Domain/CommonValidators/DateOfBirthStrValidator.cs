using FluentValidation;
using System;
using System.Globalization;

namespace Identity.API.Domain.CommonValidators
{
    public class DateOfBirthStrValidator : AbstractValidator<string>
    {
        public const string DateOfBirthFormat = "yyyy-MM-dd";

        public DateOfBirthStrValidator()
        {
            RuleFor(x => x)
                .Must(dateOfBirthStr =>
                {
                    if (!DateTime.TryParseExact(dateOfBirthStr, DateOfBirthFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOfBirth))
                    {
                        return false;
                    }

                    var dateOfBirthValidator = new DateOfBirthValidator();
                    var result = dateOfBirthValidator.Validate(dateOfBirth);
                    return result.IsValid;
                })
                .WithMessage(
                    $"Must have {DateOfBirthFormat} format and cannot be before {DateOfBirthValidator.MinDate}");
        }
    }
}
