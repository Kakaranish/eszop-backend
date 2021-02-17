using Identity.API.Application.Dto;
using MediatR;

namespace Identity.API.Application.Queries.GetProfileInfo
{
    public class GetProfileInfoQuery : IRequest<ProfileInfoDto>
    {
    }
}
