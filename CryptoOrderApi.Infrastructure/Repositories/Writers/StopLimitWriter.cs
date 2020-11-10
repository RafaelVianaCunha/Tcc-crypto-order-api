using System.Threading.Tasks;
using ExchangeApi.Domain.Repositories;
using CryptoOrderApi.Domain.Repositories;
using CryptoOrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ExchangeApi.Domain.Repositories.Writers;

namespace CryptoOrderApi.Infrastructure.Repositories.Writers
{
    public class StopLimitWriter : IStopLimitWriter
    {
        public StopLimitDbContext StopLimitDbContext { get; }

        public StopLimitWriter(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StopLimitDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            StopLimitDbContext = new StopLimitDbContext(optionsBuilder.Options);
        }

        public async Task<StopLimit> Create(StopLimit stopLimit)
        {
            await StopLimitDbContext.StopLimits.AddAsync(stopLimit);
            await StopLimitDbContext.SaveChangesAsync();

            return stopLimit;
        }

        public async Task<StopLimit> Delete(StopLimit stopLimit)
        {
            return await Update(stopLimit.Delete());
        }

        public async Task<StopLimit> Update(StopLimit stopLimit)
        {
            StopLimitDbContext.StopLimits.Attach(stopLimit);
            StopLimitDbContext.Entry(stopLimit).State = EntityState.Modified;

            await StopLimitDbContext.SaveChangesAsync();

            return stopLimit;
        }
    }
}