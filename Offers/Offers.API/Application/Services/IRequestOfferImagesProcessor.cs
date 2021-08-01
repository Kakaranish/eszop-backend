using Microsoft.AspNetCore.Http;
using Offers.Domain.Aggregates.OfferAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Offers.API.Application.Services
{
    public interface IRequestOfferImagesProcessor
    {
        public Task Process(Offer offer, IList<IFormFile> images, string imagesMetadata);
    }
}
