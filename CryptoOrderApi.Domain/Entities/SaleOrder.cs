using System;

namespace CryptoOrderApi.Domain.Entities
{
    public class SaleOrder
    {
        public Guid Id { get; set; }

        public DateTime ExecutedAt { get; set; }

        public Guid StopLimitId { get; set; }

        public StopLimit StopLimit { get; set; }
    }
}