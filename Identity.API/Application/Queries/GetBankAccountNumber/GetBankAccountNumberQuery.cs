using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Identity.API.Application.Queries.GetBankAccountNumber
{
    public class GetBankAccountNumberQuery : IRequest<string>
    {
        public string SellerId { get; init; }
    }

    public class GetBankAccountNumberQueryValidator : AbstractValidator<GetBankAccountNumberQuery>
    {
        public GetBankAccountNumberQueryValidator()
        {
            RuleFor(x => x.SellerId)
                .IsNotEmptyGuid();
        }
    }
}
