using Carts.API.Domain;
using MediatR;

namespace Carts.API.Application.Queries
{
    public class GetOrCreateCartQuery : IRequest<Cart>
    {
    }
}
