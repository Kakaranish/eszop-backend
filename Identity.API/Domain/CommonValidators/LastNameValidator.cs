﻿using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class LastNameValidator : AbstractValidator<string>
    {
        public LastNameValidator()
        {
            const string regex = @"^[a-zA-Z\p{L}\s,.'-]{3,}$";

            RuleFor(x => x)
                .NotEmpty()
                .Matches(regex)
                .WithMessage($"Must match regex {regex}");
        }
    }
}