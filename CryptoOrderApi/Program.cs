using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CryptoOrderApi
{
    public class Program
    {
        public const string APIKEY = "doM6wrGqLZqYyd0g4jA86nSmZOVjAXfM28t32XrxhgGUuzgZ9OftzYeNJLvfJMrd";
        public const string APISECRET = "9GkPW2ba27AXOwDXqfykpH4NMEF4iWzANY9MNWfIb9112JSgdoX53UlbyKA3QTGq";
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
             Binance();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

         public static void Binance(){

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(APIKEY, APISECRET),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            using (var client = new BinanceClient())
            {
                var accountInfo = client.GetAccountInfo();
                Console.WriteLine(accountInfo);
            }

         }
    }
}
