using System;

namespace CryptoOrderApi.Domain.Entities
{
    public class StopLimit
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }  
        public Decimal Stop { get; set; }
        public Decimal Limit { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public SaleOrder SaleOrder { get; set; }

        public StopLimit()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public StopLimit Delete()
        {
            DeletedAt = DateTime.UtcNow;
            return this;
        }
    }
}