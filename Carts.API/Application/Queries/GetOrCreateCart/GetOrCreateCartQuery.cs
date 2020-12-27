using Carts.API.Domain;
using MediatR;

namespace Carts.API.Application.Queries.GetOrCreateCart
{
    public class GetOrCreateCartQuery : IRequest<Cart>
    {
    }
}
