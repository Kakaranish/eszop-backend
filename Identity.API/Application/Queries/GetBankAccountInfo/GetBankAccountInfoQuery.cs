using Common.Dto;
using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Identity.API.Application.Queries.GetBankAccountInfo
{
    public class GetBankAccountInfoQuery : IRequest<BankAccountInfoDto>
    {
        public string SellerId { get; init; }
    }

    public class GetBankAccountInfoQueryValidator : AbstractValidator<GetBankAccountInfoQuery>
    {
        public GetBankAccountInfoQueryValidator()
        {
            RuleFor(x => x.SellerId)
                .IsNotEmptyGuid();
        }
    }
}
