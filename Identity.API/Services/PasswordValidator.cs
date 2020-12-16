using FluentValidation;
using System.Text.RegularExpressions;

namespace Identity.API.Services
{
    public class PasswordValidator : PasswordValidatorBase
    {
        public PasswordValidator()
        {
            static bool MinLengthLambda(string password)
                => !string.IsNullOrWhiteSpace(password) && password.Length >= 5;

            RuleFor(password => password)
                .Must(MinLengthLambda).WithMessage("Must contain at least 5 characters")
                .Matches(new Regex(@"[0-9]+")).WithMessage("Must contain at least one number")
                .Matches(new Regex(@"[A-Z]+")).WithMessage("Must contain at least one upper character");
        }
    }
}
