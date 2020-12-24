using CryptoOrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoOrderApi.Infrastructure
{
    public class SaleOrderConfiguration: IEntityTypeConfiguration<SaleOrder>
    {
     public void Configure(EntityTypeBuilder<SaleOrder> builder)
        {
            builder.ToTable("SaleOrder");
            
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExecutedAt);
            builder.Property(x => x.StopLimitId);

            builder
            .HasOne(b => b.StopLimit)
            .WithOne(i => i.SaleOrder)
            .HasForeignKey<SaleOrder>(b => b.StopLimitId);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}