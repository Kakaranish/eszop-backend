using FluentValidation;
using Identity.Domain.Aggregates.UserAggregate;

namespace Identity.Domain.Repositories.Types
{
    public class UserFilter
    {
        public int PageSize { get; init; } = 10;
        public int PageIndex { get; init; } = 1;
        public string SearchPhrase { get; init; }
        public string Role { get; set; }
    }

    public class UserFilterValidator : AbstractValidator<UserFilter>
    {
        public UserFilterValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0);

            RuleFor(x => x.PageIndex)
                .GreaterThan(0);

            RuleFor(x => x.SearchPhrase)
                .NotEmpty()
                .When(x => x.Role == null);

            RuleFor(x => x.Role)
                .Must(Role.IsValid)
                .When(x => x.Role != null)
                .WithMessage("Invalid role");
        }
    }
}
