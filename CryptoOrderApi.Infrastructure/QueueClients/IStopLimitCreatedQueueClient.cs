using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace CryptoOrderApi.Infrastructure.QueueClients
{
    public interface IStopLimitCreatedQueueClient
    {
        Task Queue(StopLimit stopLimit);
    }
}
