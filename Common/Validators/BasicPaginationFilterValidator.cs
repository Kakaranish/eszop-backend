using Common.Types;
using FluentValidation;

namespace Common.Validators
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
