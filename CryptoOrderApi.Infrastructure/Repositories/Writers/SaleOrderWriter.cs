using System.Threading.Tasks;
using ExchangeApi.Domain.Repositories;
using CryptoOrderApi.Domain.Repositories;
using CryptoOrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ExchangeApi.Domain.Repositories.Writers;

namespace CryptoOrderApi.Infrastructure.Repositories.Writers
{
    public class SaleOrderWriter : ISaleOrderWriter
    {
        public SaleOrderDbContext SaleOrderDbContext { get; }

        public async Task<SaleOrder> Create(SaleOrder saleOrder)
        {
            await SaleOrderDbContext.SaleOrder.AddAsync(saleOrder);
            await SaleOrderDbContext.SaveChangesAsync();

            return saleOrder;
        }

        public async Task<SaleOrder> Delete(SaleOrder saleOrder)
        {
            return await Update(saleOrder.Delete());
        }

        public async Task<SaleOrder> Update(SaleOrder saleOrder)
        {
            SaleOrderDbContext.SaleOrder.Attach(saleOrder);
            SaleOrderDbContext.Entry(saleOrder).State = EntityState.Modified;

            await SaleOrderDbContext.SaveChangesAsync();

            return saleOrder;
        }
    }
}