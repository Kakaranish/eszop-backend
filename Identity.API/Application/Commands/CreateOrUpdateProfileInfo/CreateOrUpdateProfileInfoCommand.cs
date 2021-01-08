using FluentValidation;
using Identity.API.Domain.CommonValidators;
using MediatR;

namespace Identity.API.Application.Commands.CreateOrUpdateProfileInfo
{
    public class CreateOrUpdateProfileInfoCommand : IRequest
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string DateOfBirth { get; init; }
        public string PhoneNumber { get; init; }
    }

    public class CreateOrUpdateProfileInfoCommandValidator : AbstractValidator<CreateOrUpdateProfileInfoCommand>
    {
        public CreateOrUpdateProfileInfoCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .SetValidator(new FirstNameValidator())
                .When(x => x.FirstName is not null);

            RuleFor(x => x.LastName)
                .SetValidator(new LastNameValidator())
                .When(x => x.LastName is not null);

            RuleFor(x => x.PhoneNumber)
                .SetValidator(new PhoneNumberValidator())
                .When(x => x.PhoneNumber is not null);

            RuleFor(x => x.DateOfBirth)
                .SetValidator(new DateOfBirthStrValidator())
                .When(x => x.DateOfBirth is not null);
        }
    }
}
