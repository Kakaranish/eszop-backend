using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using Offers.API.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.CreateOffer
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Guid>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageUploader _imageUploader;
        private readonly HttpContext _httpContext;

        public CreateOfferCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOfferRepository offerRepository, ICategoryRepository categoryRepository,
            IImageUploader imageUploader)
        {
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _imageUploader = imageUploader ?? throw new ArgumentNullException(nameof(imageUploader));
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

            foreach (var image in command.Images)
            {
                var uploadedImage = await _imageUploader.UploadAsync(image);
                offer.AddImage(uploadedImage);
            }

            await _offerRepository.AddAsync(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Task.FromResult(offer.Id);
        }
    }
}
