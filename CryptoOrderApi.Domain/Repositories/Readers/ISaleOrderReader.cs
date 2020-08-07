using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace CryptoOrderApi.Domain.Repositories.Readers
{
    public interface ISaleOrderReader
    {
        Task<SaleOrder> Get(Guid saleOrderId);

        Task<IReadOnlyCollection<SaleOrder>> Get();

        Task<bool> Any(Guid saleOrderId);
    }
}