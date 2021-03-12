using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Offers.API.Application.Types;
using Offers.API.Domain;
using Offers.API.Services;
using Offers.API.Services.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Offers.API.Application.Services
{
    public class RequestOfferImagesProcessor : IRequestOfferImagesProcessor
    {
        private readonly IImageStorage _imageStorage;
        private readonly IRequestOfferImagesMetadataExtractor _imagesMetadataExtractor;

        public RequestOfferImagesProcessor(IImageStorage imageStorage, IRequestOfferImagesMetadataExtractor imagesMetadataExtractor)
        {
            _imageStorage = imageStorage ?? throw new ArgumentNullException(nameof(imageStorage));
            _imagesMetadataExtractor = imagesMetadataExtractor ?? throw new ArgumentNullException(nameof(imagesMetadataExtractor));
        }

        public async Task Process(Offer offer, IList<IFormFile> images, string imagesMetadata)
        {
            var imagesMetadataDict = _imagesMetadataExtractor.Extract(imagesMetadata);
            if (images != null)
            {
                var imagesIdList = images.Select(img => Path.GetFileNameWithoutExtension(img.FileName));
                if (imagesIdList.Any(id => !imagesMetadataDict.ContainsKey(id)))
                    throw new OffersDomainException("Invalid images metadata");
            }

            // Delete images not present in metadata
            var imagesToRemove = offer.Images.Where(x => imagesMetadataDict.ContainsKey(x.Id.ToString()) == false);
            foreach (var imageToRemove in imagesToRemove)
            {
                await _imageStorage.DeleteAsync(imageToRemove.Filename);
                offer.RemoveImage(imageToRemove);
            }

            var newOfferImages = new List<ImageInfo>();

            // Process remaining offer images
            foreach (var offerImage in offer.Images)
            {
                var imageMetadata = imagesMetadataDict[offerImage.Id.ToString()];
                if (!imageMetadata.IsRemote)
                    throw new OffersDomainException($"Invalid metadata entry - '{nameof(imageMetadata.IsRemote)}' should be true");

                offerImage.SetIsMain(imageMetadata.IsMain);
                offerImage.SetSortId(imageMetadata.IsMain ? 0 : imageMetadata.SortId);
                imagesMetadataDict.Remove(offerImage.Id.ToString());

                newOfferImages.Add(offerImage);
            }

            offer.ClearImages();

            if (imagesMetadataDict.Any(x => x.Value.IsRemote))
                throw new OffersDomainException($"Uploaded image cannot be marked as '{nameof(ImageMetadata.IsRemote)}'");

            // Upload new images from request
            if (images != null)
            {
                foreach (var requestImageFile in images)
                {
                    var id = Path.GetFileNameWithoutExtension(requestImageFile.FileName);
                    var metadata = imagesMetadataDict[id];

                    var uploadedImage = await _imageStorage.UploadAsync(requestImageFile);
                    var uploadedImageInfo = uploadedImage.ToImageInfo();

                    uploadedImageInfo.SetIsMain(metadata.IsMain);
                    uploadedImageInfo.SetSortId(metadata.IsMain ? 0 : uploadedImageInfo.SortId);

                    newOfferImages.Add(uploadedImageInfo);
                }
            }

            var mainImage = newOfferImages.Single(x => x.IsMain);
            offer.AddImage(mainImage);
            newOfferImages.Remove(mainImage);

            foreach (var imgWithIndex in newOfferImages.OrderBy(x => x.SortId).WithIndex(1))
            {
                imgWithIndex.Item.SetSortId(imgWithIndex.Index);
                offer.AddImage(imgWithIndex.Item);
            }
        }
    }
}
