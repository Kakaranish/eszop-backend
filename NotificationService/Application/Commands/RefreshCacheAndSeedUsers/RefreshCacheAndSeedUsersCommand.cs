using MediatR;
using System;
using System.Collections.Generic;

namespace NotificationService.Application.Commands.RefreshCacheAndSeedUsers
{
    public class RefreshCacheAndSeedUsersCommand : IRequest
    {
        public IEnumerable<Guid> UserIds { get; init; }
    }
}
