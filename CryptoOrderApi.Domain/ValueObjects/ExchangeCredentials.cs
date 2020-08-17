using System;

namespace CryptoOrderApi.Domain.ValueObjects
{
    public class ExchangeCredentials
    {
        public Guid UserId { get; set; }

        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }
    }
}