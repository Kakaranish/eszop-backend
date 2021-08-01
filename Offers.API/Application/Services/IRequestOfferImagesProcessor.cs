using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Offers.Domain;
using Offers.Domain.Aggregates.OfferAggregate;

namespace Offers.API.Application.Services
{
    public interface IRequestOfferImagesProcessor
    {
        public Task Process(Offer offer, IList<IFormFile> images, string imagesMetadata);
    }
}
