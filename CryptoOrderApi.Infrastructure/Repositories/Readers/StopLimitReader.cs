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
    public class StopLimitReader : IStopLimitReader
    {
        public StopLimitDbContext StopLimitDbContext { get; }

        public StopLimitReader(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StopLimitDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            StopLimitDbContext = new StopLimitDbContext(optionsBuilder.Options);
        }

        public Task<bool> Any(Guid stopLimitId)
        {
            return StopLimitDbContext.StopLimits.AnyAsync(x => x.Id == stopLimitId);
        }

        public Task<StopLimit> Get(Guid stopLimitId)
        {
            return StopLimitDbContext.StopLimits.SingleOrDefaultAsync(x => x.Id == stopLimitId);
        }

        public async Task<IReadOnlyCollection<StopLimit>> Get()
        {
            return await StopLimitDbContext.StopLimits.Where(x => x.DeletedAt == null).ToListAsync();
        }
    }
}