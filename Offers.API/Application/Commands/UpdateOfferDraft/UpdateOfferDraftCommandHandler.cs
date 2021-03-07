using Common.Exceptions;
using Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Offers.API.Application.Types;
using Offers.API.DataAccess.Repositories;
using Offers.API.Domain;
using Offers.API.Services;
using Offers.API.Services.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Offers.API.Application.Commands.UpdateOfferDraft
{
    public class UpdateOfferDraftCommandHandler : IRequestHandler<UpdateOfferDraftCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOfferRepository _offerRepository;
        private readonly IImageStorage _imageStorage;
        private readonly ICategoryRepository _categoryRepository;

        public UpdateOfferDraftCommandHandler(IHttpContextAccessor httpContextAccessor, IOfferRepository offerRepository,
            IImageStorage imageStorage, ICategoryRepository categoryRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _imageStorage = imageStorage ?? throw new ArgumentNullException(nameof(imageStorage));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Unit> Handle(UpdateOfferDraftCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var offer = await _offerRepository.GetByIdAsync(Guid.Parse(request.OfferId));
            if (offer == null || offer.OwnerId != userId) throw new NotFoundException();

            var keyValueInfos = ExtractKeyValueInfos(request);
            offer.SetKeyValueInfos(keyValueInfos);

            await ProcessOfferImages(request, offer);

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

        private async Task ProcessOfferImages(UpdateOfferDraftCommand request, Offer offer)
        {
            var imagesMetadataDict = ExtractImagesMetadata(request);

            var imagesToRemove = offer.Images.Where(x => !imagesMetadataDict.ContainsKey(x.Id.ToString())).ToList();
            foreach (var imageToRemove in imagesToRemove)
            {
                await _imageStorage.DeleteAsync(imageToRemove.Filename);
                offer.RemoveImage(imageToRemove);
            }

            var currentOfferImages = new List<ImageInfo>();
            ImageInfo mainImage = null;

            foreach (var offerImage in offer.Images)
            {
                var imageMetadata = imagesMetadataDict[offerImage.Id.ToString()];
                if (!imageMetadata.IsRemote)
                    throw new OffersDomainException($"Invalid metadata entry - '{nameof(imageMetadata.IsRemote)}' should be true");

                offerImage.SetIsMain(imageMetadata.IsMain);
                if (offerImage.IsMain)
                {
                    offerImage.SetSortId(0);
                    mainImage = offerImage;
                }
                else
                {
                    offerImage.SetSortId(imageMetadata.SortId);
                    currentOfferImages.Add(offerImage);
                }

                imagesMetadataDict.Remove(offerImage.Id.ToString());
            }

            offer.ClearImages();

            if (imagesMetadataDict.Any(x => x.Value.IsRemote))
                throw new OffersDomainException("Uploaded image cannot be marked as isRemote");

            var uploadedImages = new List<ImageInfo>();

            if (request.Images != null)
            {
                foreach (var requestImageFile in request.Images)
                {
                    var id = Path.GetFileNameWithoutExtension(requestImageFile.FileName);
                    var metadata = imagesMetadataDict[id];

                    var uploadedImage = (await _imageStorage.UploadAsync(requestImageFile)).ToImageInfo();
                    uploadedImage.SetIsMain(metadata.IsMain);

                    if (uploadedImage.IsMain)
                    {
                        uploadedImage.SetSortId(0);
                        mainImage = uploadedImage;
                    }
                    else
                    {
                        uploadedImage.SetSortId(metadata.SortId);
                        uploadedImages.Add(uploadedImage);
                    }
                }
            }

            offer.AddImage(mainImage);
            foreach (var (offerImage, index) in
                new List<ImageInfo>(currentOfferImages).Concat(uploadedImages).OrderBy(x => x.SortId).WithIndex(1))
            {
                offerImage.SetSortId(index);
                offer.AddImage(offerImage);
            }
        }

        private static Dictionary<string, ImageMetadata> ExtractImagesMetadata(UpdateOfferDraftCommand request)
        {
            var imagesMetadataList = JsonConvert.DeserializeObject<IList<ImageMetadata>>(request.ImagesMetadata);
            var metadataDict = imagesMetadataList.ToDictionary(x => x.ImageId);

            if (imagesMetadataList == null)
                throw new OffersDomainException("Invalid images metadata");
            if (imagesMetadataList.Count == 0)
                throw new OffersDomainException("Min number of images is 1");
            if (imagesMetadataList.Count > 5)
                throw new OffersDomainException("Max number of images is 5");

            var mainImages = imagesMetadataList.Where(x => x.IsMain).ToList();
            if (mainImages.Count == 0)
                throw new OffersDomainException("No main image indicated");
            if (mainImages.Count > 1)
                throw new OffersDomainException("Possible only 1 main image");

            if (request.Images != null)
            {
                var imagesIdList = request.Images.Select(img => Path.GetFileNameWithoutExtension(img.FileName));
                if (imagesIdList.Any(id => !metadataDict.ContainsKey(id)))
                    throw new OffersDomainException("Invalid images metadata");
            }

            return metadataDict;
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
