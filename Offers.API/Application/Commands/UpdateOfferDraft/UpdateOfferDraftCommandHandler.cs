using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.Application.Services;
using Offers.API.DataAccess.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.UpdateOfferDraft
{
    public class UpdateOfferDraftCommandHandler : IRequestHandler<UpdateOfferDraftCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOfferRepository _offerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRequestOfferImagesProcessor _offerImagesProcessor;
        private readonly IRequestDeliveryMethodExtractor _deliveryMethodExtractor;
        private readonly IRequestKeyValueInfoExtractor _keyValueInfoExtractor;

        public UpdateOfferDraftCommandHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository,
            ICategoryRepository categoryRepository, IRequestOfferImagesProcessor offerImagesProcessor,
            IRequestDeliveryMethodExtractor deliveryMethodExtractor, IRequestKeyValueInfoExtractor keyValueInfoExtractor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _offerImagesProcessor = offerImagesProcessor ?? throw new ArgumentNullException(nameof(offerImagesProcessor));
            _deliveryMethodExtractor = deliveryMethodExtractor ?? throw new ArgumentNullException(nameof(deliveryMethodExtractor));
            _keyValueInfoExtractor = keyValueInfoExtractor ?? throw new ArgumentNullException(nameof(keyValueInfoExtractor));
        }

        public async Task<Unit> Handle(UpdateOfferDraftCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var offer = await _offerRepository.GetByIdAsync(Guid.Parse(request.OfferId));
            if (offer == null || offer.OwnerId != userId) throw new NotFoundException();

            var keyValueInfos = _keyValueInfoExtractor.Extract(request.KeyValueInfos);
            offer.SetKeyValueInfos(keyValueInfos);

            await _offerImagesProcessor.Process(offer, request.Images, request.ImagesMetadata);

            if (request.Name != null) offer.SetName(request.Name);
            if (request.Description != null) offer.SetDescription(request.Description);
            if (request.Price != null) offer.SetPrice(request.Price.Value);
            if (request.TotalStock.HasValue) offer.SetTotalStock(request.TotalStock.Value);
            if (request.CategoryId != null)
            {
                var categoryId = Guid.Parse(request.CategoryId);
                if (categoryId != offer.Category.Id)
                {
                    var category = await _categoryRepository.GetByIdAsync(categoryId);
                    if (category == null) throw new NotFoundException("Category");
                    offer.SetCategory(category);
                }
            }

            var deliveryMethods = _deliveryMethodExtractor.Extract(request.DeliveryMethods);
            offer.SetDeliveryMethods(deliveryMethods);

            _offerRepository.Update(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
