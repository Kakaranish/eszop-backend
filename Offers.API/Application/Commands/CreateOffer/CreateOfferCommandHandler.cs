using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.CreateOffer
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Guid>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly HttpContext _httpContext;

        public CreateOfferCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOfferRepository offerRepository, ICategoryRepository categoryRepository)
        {
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Guid> Handle(CreateOfferCommand command, CancellationToken cancellationToken)
        {
            var categoryId = Guid.Parse(command.CategoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is null) throw new OffersDomainException($"There is no category with id {categoryId}");

            var tokenPayload = _httpContext.User.Claims.ToTokenPayload();
            
            var offer = new Offer(
                ownerId: tokenPayload.UserClaims.Id,
                name: command.Name,
                description: command.Description,
                price: command.Price,
                totalStock: command.TotalStock, 
                category: category
            );

            await _offerRepository.AddAsync(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Task.FromResult(offer.Id);
        }
    }
}
