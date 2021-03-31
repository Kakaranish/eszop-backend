using Common.Extensions;
using Common.ImageStorage;
using Microsoft.AspNetCore.Http;
using Offers.API.Application.Types;
using Offers.API.Domain;
using Offers.API.Extensions;
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
            var imagesToRemove = offer.Images.Where(x => imagesMetadataDict.ContainsKey(x.Id.ToString()) == false).ToList();
            foreach (var imageToRemove in imagesToRemove)
            {
                await _imageStorage.DeleteAsync(imageToRemove.Filename);
            }

            var currentImages = offer.Images.Except(imagesToRemove).Select(x => (ImageInfo)x.Clone()).ToList();

            // Process remaining offer images
            foreach (var offerImage in currentImages)
            {
                var imageMetadata = imagesMetadataDict[offerImage.Id.ToString()];
                if (!imageMetadata.IsRemote)
                    throw new OffersDomainException($"Invalid metadata entry - '{nameof(imageMetadata.IsRemote)}' should be true");

                offerImage.SetIsMain(imageMetadata.IsMain);
                offerImage.SetSortId(imageMetadata.IsMain ? 0 : imageMetadata.SortId);
                imagesMetadataDict.Remove(offerImage.Id.ToString());
            }

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
                    if (uploadedImage == null) throw new OffersDomainException("Image upload failed");
                    var uploadedImageInfo = uploadedImage.ToImageInfo();

                    uploadedImageInfo.SetIsMain(metadata.IsMain);
                    uploadedImageInfo.SetSortId(metadata.IsMain ? 0 : uploadedImageInfo.SortId);

                    currentImages.Add(uploadedImageInfo);
                }
            }

            var mainImage = currentImages.Single(x => x.IsMain);
            currentImages.Remove(mainImage);

            var finalImages = new List<ImageInfo>();
            foreach (var imgWithIndex in currentImages.OrderBy(x => x.SortId).WithIndex(1))
            {
                imgWithIndex.Item.SetSortId(imgWithIndex.Index);
                finalImages.Add(imgWithIndex.Item);
            }
            finalImages.Insert(0, mainImage);

            offer.SetImages(finalImages);
        }
    }
}
