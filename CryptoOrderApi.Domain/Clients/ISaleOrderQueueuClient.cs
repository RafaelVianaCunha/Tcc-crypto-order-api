using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace CryptoOrderApi.Domain.Clients
{
    public interface ISaleOrderQueueClient
    {
        Task Queue(SaleOrder saleOrder);
    }
}