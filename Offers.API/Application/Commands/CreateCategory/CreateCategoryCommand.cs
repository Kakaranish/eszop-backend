using FluentValidation;
using MediatR;
using System;
using Offers.Domain.Validators;

namespace Offers.API.Application.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Guid>
    {
        public string Name { get; set; }
    }

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .SetValidator(new CategoryNameValidator());
        }
    }
}
