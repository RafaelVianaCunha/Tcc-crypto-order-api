using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace CryptoOrderApi.Domain.Repositories 
{
    public interface IStopLimitReader
    {
        Task<StopLimit> Get(Guid stopLimitId);

        Task<IReadOnlyCollection<StopLimit>> Get();

        Task<bool> Any(Guid stopLimitId);
    }
}