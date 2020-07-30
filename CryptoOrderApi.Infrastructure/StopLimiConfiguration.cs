using CryptoOrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace CryptoOrderApi.Infrastructure
{
    public class StopLimiConfiguration : IEntityTypeConfiguration<StopLimit>
    {
        public void Configure(EntityTypeBuilder<StopLimit> builder)
        {
            builder.ToTable("StopLimits");
            
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.UserID);
            builder.Property(x => x.Stop);
            builder.Property(x => x.Limit);
            builder.Property(x => x.Quantity);
            builder.Property(x => x.CreatedAt);
            builder.Property(x => x.DeletedAt);
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