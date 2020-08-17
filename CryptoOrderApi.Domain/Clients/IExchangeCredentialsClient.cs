using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Domain.ValueObjects;
using System;

namespace CryptoOrderApi.Domain.Clients
{
    public interface IExchangeCredentialsClient
    {
        Task<ExchangeCredentials> Get(Guid userId);
    }
}