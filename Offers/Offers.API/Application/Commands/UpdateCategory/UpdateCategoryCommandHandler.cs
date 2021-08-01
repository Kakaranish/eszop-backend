using Common.Utilities.Exceptions;
using Common.Utilities.Extensions;
using Common.Utilities.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Offers.Domain.Exceptions;
using Offers.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ILogger<UpdateCategoryCommandHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateCategoryCommandHandler(ILogger<UpdateCategoryCommandHandler> logger,
            ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryId = Guid.Parse(request.CategoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null) throw new NotFoundException();
            if (category.Name == request.Name) return await Unit.Task;

            var otherCategory = await _categoryRepository.GetByNameAsync(request.Name);
            if (otherCategory != null) throw new OffersDomainException("Other category has the same name");

            var previousName = category.Name;
            category.SetName(request.Name);

            _categoryRepository.Update(category);
            await _categoryRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            _logger.LogWithProps(LogLevel.Information, "Category name updated",
                "CategoryId".ToKvp(categoryId),
                "PreviousName".ToKvp(previousName),
                "NewName".ToKvp(request.Name),
                "UserId".ToKvp(userId));

            return await Unit.Task;
        }
    }
}
