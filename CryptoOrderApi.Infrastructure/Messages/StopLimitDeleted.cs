using System;

namespace CryptoOrderApi.Infrastructure.Messages
{
    public class StopLimitDeleted
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }  
        public Decimal Stop { get; set; }
        public Decimal Limit { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}