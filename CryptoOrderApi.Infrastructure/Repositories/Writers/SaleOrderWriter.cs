using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ExchangeApi.Domain.Repositories.Writers;

namespace CryptoOrderApi.Infrastructure.Repositories.Writers
{
    public class SaleOrderWriter : ISaleOrderWriter
    {
        public StopLimitDbContext StopLimitDbContext { get; }

        public SaleOrderWriter(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StopLimitDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            StopLimitDbContext = new StopLimitDbContext(optionsBuilder.Options);
        }

        public async Task<SaleOrder> Create(SaleOrder saleOrder)
        {
            await StopLimitDbContext.SaleOrder.AddAsync(saleOrder);
            await StopLimitDbContext.SaveChangesAsync();

            return saleOrder;
        }

        public async Task<SaleOrder> Delete(SaleOrder saleOrder)
        {
            return await Update(saleOrder.Delete());
        }

        public async Task<SaleOrder> Update(SaleOrder saleOrder)
        {
            StopLimitDbContext.SaleOrder.Attach(saleOrder);
            StopLimitDbContext.Entry(saleOrder).State = EntityState.Modified;

            await StopLimitDbContext.SaveChangesAsync();

            return saleOrder;
        }
    }
}