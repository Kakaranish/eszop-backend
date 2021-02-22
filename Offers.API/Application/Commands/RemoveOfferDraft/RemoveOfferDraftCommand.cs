using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Offers.API.Application.Commands.RemoveOfferDraft
{
    public class RemoveOfferDraftCommand : IRequest
    {
        public string OfferId { get; set; }
    }

    public class RemoveOfferDraftCommandValidator : AbstractValidator<RemoveOfferDraftCommand>
    {
        public RemoveOfferDraftCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();
        }
    }
}
