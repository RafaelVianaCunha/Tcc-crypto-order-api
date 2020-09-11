using CryptoOrderApi.Domain.Clients;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Domain.ValueObjects;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CryptoOrderApi.Infrastructure.Clients
{
    public class ExchangeCredentialsClient : IExchangeCredentialsClient
    {
        public HttpClient HttpClient { get; } 

        public string ExchangeApiUrl { get; }

        public ExchangeCredentialsClient(
            HttpClient httpClient, string exchangeApiUrl)
        {
            HttpClient = httpClient;
            ExchangeApiUrl = exchangeApiUrl;
        }

        public async Task<ExchangeCredentials> Get(Guid userId)
        {
            var httpResponse = await HttpClient.GetAsync($"{ExchangeApiUrl}/api/exchanges/users/{userId}/credentials");

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve tasks");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var exchangeCredentials = JsonConvert.DeserializeObject<ExchangeCredentials>(content);

            return exchangeCredentials;
        }
    }
}