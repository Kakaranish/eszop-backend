using Common.Domain.Validators;
using FluentValidation;
using Identity.API.Domain.CommonValidators;
using MediatR;

namespace Identity.API.Application.Commands.CreateOrUpdateSellerInfo
{
    public class CreateOrUpdateSellerInfoCommand : IRequest
    {
        public string ContactEmail { get; init; }
        public string PhoneNumber { get; init; }
        public string BankAccountNumber { get; init; }
        public string AdditionalInfo { get; init; }
    }

    public class CreateOrUpdateSellerInfoCommandValidator : AbstractValidator<CreateOrUpdateSellerInfoCommand>
    {
        public CreateOrUpdateSellerInfoCommandValidator()
        {
            RuleFor(x => x.ContactEmail)
                .SetValidator(new EmailValidator())
                .When(x => !string.IsNullOrWhiteSpace(x.ContactEmail));

            RuleFor(x => x.PhoneNumber)
                .SetValidator(new PhoneNumberValidator())
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.BankAccountNumber)
                .SetValidator(new BankAccountNumberValidator())
                .When(x => !string.IsNullOrWhiteSpace(x.BankAccountNumber));
        }
    }
}
