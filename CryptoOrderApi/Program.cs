using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoMonitor.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CryptoOrderApi
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        
        public static void Main(string[] args)
        {
            GetAppSettingsFile();
            var serviceBusConnectionString = Configuration.GetSection("ServiceBusConnectionString").Value;
            
            var newStopLimitOrderSaleQueueName = Configuration.GetSection("NewStopLimitOrderSaleQueue").Value;
            var newStopLimitOrderSaleQueueClient = new QueueClient(serviceBusConnectionString, newStopLimitOrderSaleQueueName);

            var saleOrderExecutedQueueName = Configuration.GetSection("SaleOrderExecutedQueue").Value;
            var saleOrderExecutedQueueClient = new QueueClient(serviceBusConnectionString, saleOrderExecutedQueueName);

            var stopLimitCreatedQueueName = Configuration.GetSection("StopLimitCreatedQueue").Value;
            var stopLimitCreatedQueueClient = new QueueClient(serviceBusConnectionString, stopLimitCreatedQueueName);

            var stopLimitDeletedQueueName = Configuration.GetSection("StopLimitDeletedQueue").Value;
            var stopLimitDeletedQueueClient = new QueueClient(serviceBusConnectionString, stopLimitDeletedQueueName);

            
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }
    }
}
