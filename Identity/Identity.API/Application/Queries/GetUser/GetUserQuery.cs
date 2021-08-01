using Common.Utilities.Extensions;
using FluentValidation;
using Identity.API.Application.Dto;
using MediatR;

namespace Identity.API.Application.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserPreviewDto>
    {
        public string UserId { get; set; }
    }

    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .IsNotEmptyGuid();
        }
    }
}
