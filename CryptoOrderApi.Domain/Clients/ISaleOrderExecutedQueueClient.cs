using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace CryptoOrderApi.Domain.Clients
{
    public interface ISaleOrderExecutedQueueClient
    {
        Task Queue(SaleOrder saleOrder);
    }
}