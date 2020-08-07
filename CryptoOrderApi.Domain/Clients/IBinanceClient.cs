using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace CryptoOrderApi.Domain.Clients
{
    public interface IBinanceClient
    {
        Task PlaceOrder(SaleOrder saleOrder);
    }
}