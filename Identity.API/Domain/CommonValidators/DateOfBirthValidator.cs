using FluentValidation;
using System;

namespace Identity.API.Domain.CommonValidators
{
    public class DateOfBirthValidator : AbstractValidator<DateTime>
    {
        public static DateTime MinDate => new(1930, 1, 1);
        public DateOfBirthValidator()
        {
            RuleFor(x => x)
                .Must(x => x > MinDate)
                .WithMessage($"Min date of birth is {MinDate}");
        }
    }
}
