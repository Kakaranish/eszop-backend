using Common.Extensions;
using FluentValidation;
using MediatR;
using Offers.API.Domain.Validators;

namespace Offers.API.Application.Commands.UpdateOfferDraftTwo
{
    public class UpdateOfferDraftTwoCommand : IRequest
    {
        public string OfferId { get; init; }
        public string DeliveryMethods { get; init; }
        public string BankAccount { get; init; }
    }

    public class UpdateOfferDraftTwoCommandValidator : AbstractValidator<UpdateOfferDraftTwoCommand>
    {
        public UpdateOfferDraftTwoCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();

            RuleFor(x => x.DeliveryMethods)
                .NotEmpty();

            RuleFor(x => x.BankAccount)
                .SetValidator(new BankAccountNumberValidator());
        }
    }
}
