using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace ExchangeApi.Domain.Repositories.Writers
{
    public interface IStopLimitWriter
    {
        Task<StopLimit> Create(StopLimit stopLimit);

        Task<StopLimit> Update(StopLimit stopLimit);

        Task<StopLimit> Delete(StopLimit stopLimit);
    }
}