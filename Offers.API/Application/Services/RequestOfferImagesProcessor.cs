using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

        public RequestOfferImagesProcessor(IImageStorage imageStorage)
        {
            _imageStorage = imageStorage ?? throw new ArgumentNullException(nameof(imageStorage));
        }

        public async Task Process(Offer offer, IList<IFormFile> images, string imagesMetadata)
        {
            var imagesMetadataDict = ExtractImagesMetadata(imagesMetadata, images);

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

            if (images != null)
            {
                foreach (var requestImageFile in images)
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

        private static Dictionary<string, ImageMetadata> ExtractImagesMetadata(string imagesMetadata, IList<IFormFile> images)
        {
            var imagesMetadataList = JsonConvert.DeserializeObject<IList<ImageMetadata>>(imagesMetadata);
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

            if (images != null)
            {
                var imagesIdList = images.Select(img => Path.GetFileNameWithoutExtension(img.FileName));
                if (imagesIdList.Any(id => !metadataDict.ContainsKey(id)))
                    throw new OffersDomainException("Invalid images metadata");
            }

            return metadataDict;
        }
    }
}
