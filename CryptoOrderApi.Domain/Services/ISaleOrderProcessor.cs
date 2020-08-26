using System;
using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;

namespace CryptoOrderApi.Domain.Services
{
    public interface ISaleOrderProcessor 
    {
        Task<SaleOrder> Sell(Guid stopLimitId);    
    }
}
