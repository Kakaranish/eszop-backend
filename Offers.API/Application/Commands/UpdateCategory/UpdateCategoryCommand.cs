using Common.Extensions;
using FluentValidation;
using MediatR;
using Offers.Domain.Validators;

namespace Offers.API.Application.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public string CategoryId { get; set; }
        public string Name { get; init; }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryId)
                .IsNotEmptyGuid();

            RuleFor(x => x.Name)
                .SetValidator(new CategoryNameValidator());
        }
    }
}
