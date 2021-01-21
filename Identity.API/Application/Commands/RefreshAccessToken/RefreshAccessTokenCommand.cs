using MediatR;

namespace Identity.API.Application.Commands.RefreshAccessToken
{
    public class RefreshAccessTokenCommand : IRequest<string>
    {
    }
}
