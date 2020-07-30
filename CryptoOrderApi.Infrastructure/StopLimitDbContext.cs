using System;
using CryptoOrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoOrderApi.Infrastructure
{
    public class StopLimitDbContext : DbContext , IDisposable
    {
        public DbSet<StopLimit> StopLimits { get; set; }

        public DbSet<SaleOrder> SaleOrder { get; set; }

        protected StopLimitDbContext(){
            
        }

        protected StopLimitDbContext(DbContextOptions options) : base(options)
        {/*  */
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StopLimiConfiguration());
            modelBuilder.ApplyConfiguration(new SaleOrderConfiguration());
        }
    
    }

}