using MediatR;
using System;

namespace Carts.API.Application.Commands.FinalizeCart
{
    public class FinalizeCartCommand : IRequest<Guid>
    {
    }
}
