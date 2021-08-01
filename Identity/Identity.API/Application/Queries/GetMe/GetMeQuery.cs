using Identity.API.Application.Dto;
using MediatR;

namespace Identity.API.Application.Queries.GetMe
{
    public class GetMeQuery : IRequest<MeDto>
    {
    }
}
