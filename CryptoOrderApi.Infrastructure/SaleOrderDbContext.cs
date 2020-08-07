using System;
using CryptoOrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoOrderApi.Infrastructure
{
    public class SaleOrderDbContext : DbContext , IDisposable
    {
        public DbSet<SaleOrder> SaleOrders { get; set; }

        public DbSet<SaleOrder> SaleOrder { get; set; }

        protected SaleOrderDbContext(){
            
        }

        protected SaleOrderDbContext(DbContextOptions options) : base(options)
        {/*  */
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StopLimiConfiguration());
            modelBuilder.ApplyConfiguration(new SaleOrderConfiguration());
        }
    
    }

}