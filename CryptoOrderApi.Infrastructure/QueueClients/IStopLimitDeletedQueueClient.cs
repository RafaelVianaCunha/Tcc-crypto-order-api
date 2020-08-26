using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace CryptoOrderApi.Infrastructure.QueueClients
{
    public interface IStopLimitDeletedQueueClient
    {
        Task Queue(StopLimit stopLimit);
    }
}
