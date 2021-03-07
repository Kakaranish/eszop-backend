using Common.Types;
using FluentValidation;
using Identity.API.Application.Dto;
using Identity.API.Application.Types;
using MediatR;

namespace Identity.API.Application.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<Pagination<UserPreviewDto>>
    {
        public UserFilter UserFilter { get; init; }
    }

    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.UserFilter)
                .SetValidator(new UserFilterValidator());
        }
    }
}
