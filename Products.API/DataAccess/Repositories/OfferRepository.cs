using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.API.Domain;

namespace Products.API.DataAccess.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly AppDbContext _appDbContext;

        public OfferRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task AddAsync(Offer offer)
        {
            _appDbContext.Offers.Add(offer);
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Offer> GetAll()
        {
            return _appDbContext.Offers;
        }
    }
}
