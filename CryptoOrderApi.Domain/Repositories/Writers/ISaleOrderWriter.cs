using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;


namespace ExchangeApi.Domain.Repositories.Writers
{
    public interface ISaleOrderWriter
    {
        Task<SaleOrder> Create(SaleOrder saleOrder);

        Task<SaleOrder> Update(SaleOrder saleOrder);

        Task<SaleOrder> Delete(SaleOrder saleOrder);
    }
}