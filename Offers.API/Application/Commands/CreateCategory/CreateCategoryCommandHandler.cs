using MediatR;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByNameAsync(request.Name);
            if (category != null)
            {
                throw new OffersDomainException($"Category {category.Id} has the same name");
            }

            category = new Category(request.Name);
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return category.Id;
        }
    }
}
