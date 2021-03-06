using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Domain.ValueObjects;

namespace CryptoOrderApi.Domain.Clients
{
    public interface IBinanceClient
    {
        void PlaceOrder(ExchangeCredentials exchangeCredentials, SaleOrder saleOrder);
    }
}