using Common.Dto;
using MediatR;

namespace Identity.API.Application.Queries.GetBankAccountInfo
{
    public class GetMyBankAccountInfoQuery : IRequest<BankAccountInfoDto>
    {
    }
}
