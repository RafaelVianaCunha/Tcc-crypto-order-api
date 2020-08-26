using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using CryptoOrderApi.Domain.Repositories.Readers;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CryptoOrderApi.Infrastructure.Repositories.Readers
{
    public class SaleOrderReader : ISaleOrderReader
    {
        public StopLimitDbContext StopLimitDbContext { get; }

        public SaleOrderReader(StopLimitDbContext stopLimitDbContext)
        {
            StopLimitDbContext = stopLimitDbContext;
        }

        public Task<bool> Any(Guid saleOrderId)
        {
            return StopLimitDbContext.SaleOrders.AnyAsync(x => x.Id == saleOrderId);

        }

        public Task<SaleOrder> Get(Guid saleOrderId)
        {
            return StopLimitDbContext.SaleOrders.SingleOrDefaultAsync(x => x.Id == saleOrderId);

        }

        public async Task<IReadOnlyCollection<SaleOrder>> Get()
        {
          return await StopLimitDbContext.SaleOrders.Where(x => x.DeletedAt == null).ToListAsync();
        }
    }
}