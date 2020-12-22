﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Offers.API.Domain;

namespace Offers.API.DataAccess.Repositories
{
    public interface IOfferRepository
    {
        Task AddAsync(Offer product);
        Task<IList<Offer>> GetAllAsync();
    }
}