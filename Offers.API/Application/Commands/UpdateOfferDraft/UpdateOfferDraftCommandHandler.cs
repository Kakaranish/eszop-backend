using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Offers.API.Application.Services;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using System;
using System.Collections.Generic;
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

        public UpdateOfferDraftCommandHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository, 
            ICategoryRepository categoryRepository, IRequestOfferImagesProcessor offerImagesProcessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _offerImagesProcessor = offerImagesProcessor ?? throw new ArgumentNullException(nameof(offerImagesProcessor));
        }

        public async Task<Unit> Handle(UpdateOfferDraftCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var offer = await _offerRepository.GetByIdAsync(Guid.Parse(request.OfferId));
            if (offer == null || offer.OwnerId != userId) throw new NotFoundException();

            var keyValueInfos = ExtractKeyValueInfos(request);
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

            var deliveryMethods = ExtractDeliveryMethods(request);
            offer.SetDeliveryMethods(deliveryMethods);

            _offerRepository.Update(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }

        private static IList<KeyValueInfo> ExtractKeyValueInfos(UpdateOfferDraftCommand request)
        {
            if (request.KeyValueInfos == null) return null;

            var extractKeyValueInfos = JsonConvert.DeserializeObject<IList<KeyValueInfo>>(request.KeyValueInfos)
                                       ?? throw new OffersDomainException($"'{request.KeyValueInfos}' is not parsable");

            return extractKeyValueInfos;
        }

        private static IList<DeliveryMethod> ExtractDeliveryMethods(UpdateOfferDraftCommand request)
        {
            var extractedDeliveryMethods = JsonConvert.DeserializeObject<IList<DeliveryMethod>>(request.DeliveryMethods)
                                           ?? throw new OffersDomainException($"'{request.DeliveryMethods}' is not parsable");

            return extractedDeliveryMethods;
        }
    }
}
