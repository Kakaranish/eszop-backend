using FluentValidation;

namespace Identity.API.Services
{
    public abstract class PasswordValidatorBase : AbstractValidator<string>
    {
    }
}
