using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;
using Orders.API.Application.Dto;

namespace Orders.API.Application.Commands.GetBankTransferDetails
{
    public class GetBankTransferDetailsCommand : IRequest<BankTransferDetailsDto>
    {
        public string OrderId { get; init; }
    }

    public class GetBankTransferDetailsCommandValidator : AbstractValidator<GetBankTransferDetailsCommand>
    {
        public GetBankTransferDetailsCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .IsNotEmptyGuid();
        }
    }
}
