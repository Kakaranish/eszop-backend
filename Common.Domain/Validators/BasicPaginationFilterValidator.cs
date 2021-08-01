using Common.Domain.Types;
using FluentValidation;

namespace Common.Domain.Validators
{
    public class BasicPaginationFilterValidator : AbstractValidator<BasicPaginationFilter>
    {
        public BasicPaginationFilterValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0);

            RuleFor(x => x.PageIndex)
                .GreaterThan(0);
        }
    }
}
