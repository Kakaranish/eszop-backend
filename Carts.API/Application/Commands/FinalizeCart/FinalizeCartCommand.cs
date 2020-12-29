using System;
using MediatR;

namespace Carts.API.Application.Commands.FinalizeCart
{
    public class FinalizeCartCommand : IRequest<Guid>
    {
    }
}
