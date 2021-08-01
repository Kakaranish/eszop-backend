using System;
using FluentValidation;

namespace Common.Domain.Validators
{
    public class IdValidator : AbstractValidator<Guid>
    {
        public IdValidator()
        {
            RuleFor(x => x)
                .Must(x => x != Guid.Empty)
                .WithMessage("Cannot be empty guid");
        }
    }
}
