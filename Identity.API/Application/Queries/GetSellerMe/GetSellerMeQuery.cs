using Identity.API.Application.Dto;
using MediatR;

namespace Identity.API.Application.Queries.GetSellerMe
{
    public class GetSellerMeQuery : IRequest<SellerInfoDto>
    {
    }
}
