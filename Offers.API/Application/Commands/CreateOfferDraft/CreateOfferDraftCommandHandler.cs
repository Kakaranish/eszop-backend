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

namespace Offers.API.Application.Commands.CreateOfferDraft
{
    public class CreateOfferDraftCommandHandler : IRequestHandler<CreateOfferDraftCommand, Guid>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageStorage _imageStorage;
        private readonly HttpContext _httpContext;

        public CreateOfferDraftCommandHandler(IHttpContextAccessor httpContextAccessor,
            IOfferRepository offerRepository, ICategoryRepository categoryRepository,
            IImageStorage imageStorage)
        {
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
            _offerRepository = offerRepository ?? throw new ArgumentNullException(nameof(offerRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _imageStorage = imageStorage ?? throw new ArgumentNullException(nameof(imageStorage));
        }

        public async Task<Guid> Handle(CreateOfferDraftCommand request, CancellationToken cancellationToken)
        {
            var categoryId = Guid.Parse(request.CategoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category is null) throw new OffersDomainException($"There is no category with id {categoryId}");

            var tokenPayload = _httpContext.User.Claims.ToTokenPayload();

            var offer = new Offer(
                ownerId: tokenPayload.UserClaims.Id,
                name: request.Name,
                description: request.Description,
                price: request.Price,
                totalStock: request.TotalStock,
                category: category
            );

            var keyValueInfos = ExtractKeyValueInfos(request);
            offer.SetKeyValueInfos(keyValueInfos);

            await ProcessOfferImages(request, offer);

            await _offerRepository.AddAsync(offer);
            await _offerRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Task.FromResult(offer.Id);
        }

        private async Task ProcessOfferImages(CreateOfferDraftCommand request, Offer offer)
        {
            var imagesToUpload = ExtractImagesToUpload(request);
            var uploadedImages = await UploadImages(imagesToUpload);
            foreach (var uploadedImage in uploadedImages)
            {
                offer.AddImage(uploadedImage);
            }
        }

        private async Task<IList<ImageInfo>> UploadImages(IList<ImageToUpload> imagesToUpload)
        {
            var uploadedImages = new List<ImageInfo>();

            var mainImage = imagesToUpload[0];
            var uploadedMainImage = (await _imageStorage.UploadAsync(mainImage.File)).ToImageInfo();
            uploadedMainImage.SetIsMain(true);
            uploadedMainImage.SetSortId(0);
            uploadedImages.Add(uploadedMainImage);

            foreach (var (imageToUpload, index) in imagesToUpload.Skip(1).WithIndex(1))
            {
                var uploadedImage = (await _imageStorage.UploadAsync(imageToUpload.File)).ToImageInfo();
                uploadedImage.SetSortId(index);
                uploadedImages.Add(uploadedImage);
            }

            return uploadedImages;
        }

        private static IList<ImageToUpload> ExtractImagesToUpload(CreateOfferDraftCommand request)
        {
            var imagesMetadata = ExtractImagesMetadata(request);
            var imagesToUpload = new List<ImageToUpload>();

            ImageToUpload mainImage = null;
            foreach (var requestImage in request.Images)
            {
                var id = Path.GetFileNameWithoutExtension(requestImage.FileName);
                var metadata = imagesMetadata[id];

                var imageToUpload = new ImageToUpload
                {
                    Id = id,
                    File = requestImage,
                    Metadata = metadata
                };

                if (metadata.IsMain) mainImage = imageToUpload;
                else imagesToUpload.Add(imageToUpload);
            }

            imagesToUpload = imagesToUpload.OrderBy(x => x.Metadata.SortId).ToList();
            imagesToUpload.Insert(0, mainImage);

            return imagesToUpload;
        }

        private static Dictionary<string, ImageMetadata> ExtractImagesMetadata(CreateOfferDraftCommand request)
        {
            var imagesMetadataList = JsonConvert.DeserializeObject<IList<ImageMetadata>>(request.ImagesMetadata);
            if (imagesMetadataList == null) throw new OffersDomainException("Invalid images metadata");

            var metadataDict = imagesMetadataList.ToDictionary(x => x.ImageId);

            if (!imagesMetadataList.Any(img => img.IsMain))
                throw new OffersDomainException("No main image indicated");

            var imagesIdList = request.Images.Select(img => Path.GetFileNameWithoutExtension(img.FileName));
            if (imagesIdList.Any(id => !metadataDict.ContainsKey(id)))
                throw new OffersDomainException("Invalid images metadata");

            return metadataDict;
        }

        private static IList<KeyValueInfo> ExtractKeyValueInfos(CreateOfferDraftCommand request)
        {
            if (request.KeyValueInfos == null) return null;

            var extractKeyValueInfos = JsonConvert.DeserializeObject<IList<KeyValueInfo>>(request.KeyValueInfos)
                                       ?? throw new OffersDomainException($"'{request.KeyValueInfos}' is not parsable");

            return extractKeyValueInfos;
        }
    }
}
