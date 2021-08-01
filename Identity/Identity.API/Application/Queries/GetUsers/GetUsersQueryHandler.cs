using Common.Domain.Types;
using Common.Utilities.Extensions;
using Identity.API.Application.Dto;
using Identity.API.Extensions;
using Identity.Domain.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Pagination<UserPreviewDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Pagination<UserPreviewDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var usersPagination = await _userRepository.GetUsers(request.UserFilter);

            var usersDtoPagination = usersPagination.Transform(users =>
                users.Select(user => user.ToPreviewDto()));

            return usersDtoPagination;
        }
    }
}
