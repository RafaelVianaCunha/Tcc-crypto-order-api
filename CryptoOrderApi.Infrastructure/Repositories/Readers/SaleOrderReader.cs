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
        public SaleOrderDbContext SaleOrderDbContext { get; }

        public Task<bool> Any(Guid saleOrderId)
        {
            return SaleOrderDbContext.SaleOrder.AnyAsync(x => x.Id == saleOrderId);

        }

        public Task<SaleOrder> Get(Guid saleOrderId)
        {
            return SaleOrderDbContext.SaleOrder.SingleOrDefaultAsync(x => x.Id == saleOrderId);

        }

        public async Task<IReadOnlyCollection<SaleOrder>> Get()
        {
          return await SaleOrderDbContext.SaleOrder.Where(x => x.DeletedAt != null).ToListAsync();
        }
    }
}