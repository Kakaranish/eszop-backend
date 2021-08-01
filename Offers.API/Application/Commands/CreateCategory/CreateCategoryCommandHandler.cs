using Common.Extensions;
using Common.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Offers.Domain;
using Offers.Domain.Aggregates.CategoryAggregate;
using Offers.Domain.Exceptions;
using Offers.Domain.Repositories;

namespace Offers.API.Application.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCategoryCommandHandler(ILogger<CreateCategoryCommandHandler> logger,
            ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByNameAsync(request.Name);
            if (category != null) throw new OffersDomainException("Other category has the same name");

            category = new Category(request.Name);
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            _logger.LogWithProps(LogLevel.Information, "Category created",
                "CategoryId".ToKvp(category.Id),
                "Name".ToKvp(request.Name),
                "UserId".ToKvp(userId));

            return category.Id;
        }
    }
}
