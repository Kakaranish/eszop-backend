using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Offers.Domain;
using Offers.Domain.Aggregates.OfferAggregate;
using Offers.Domain.Repositories;

namespace Offers.API.Application.Commands.CreateOfferDraft
{
    public class CreateOfferDraftCommandHandler : IRequestHandler<CreateOfferDraftCommand, Guid>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOfferRepository _offerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRequestOfferImagesProcessor _offerImagesProcessor;
        private readonly IRequestDeliveryMethodExtractor _deliveryMethodExtractor;
        private readonly IRequestKeyValueInfoExtractor _keyValueInfoExtractor;

        public CreateOfferDraftCommandHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository,
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

        public async Task<Guid> Handle(CreateOfferDraftCommand request, CancellationToken cancellationToken)
        {
            var categoryId = Guid.Parse(request.CategoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null) throw new NotFoundException("Category");

            var tokenPayload = _httpContextAccessor.HttpContext!.User.Claims.ToTokenPayload();

            var offer = new Offer(
                ownerId: tokenPayload.UserClaims.Id,
                name: request.Name,
                description: request.Description,
                price: request.Price,
                totalStock: request.TotalStock,
                category: category
            );

            var keyValueInfos = _keyValueInfoExtractor.Extract(request.KeyValueInfos);
            offer.SetKeyValueInfos(keyValueInfos);

            var deliveryMethods = _deliveryMethodExtractor.Extract(request.DeliveryMethods);
            offer.SetDeliveryMethods(deliveryMethods);

            await _offerImagesProcessor.Process(offer, request.Images, request.ImagesMetadata);

            await _offerRepository.AddAsync(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Task.FromResult(offer.Id);
        }
    }
}