using Carts.API.Application.Dto;
using MediatR;

namespace Carts.API.Application.Queries.GetOrCreateCart
{
    public class GetOrCreateCartQuery : IRequest<CartDto>
    {
    }
}
