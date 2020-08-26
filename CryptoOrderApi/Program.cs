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
        static IQueueClient queueClient;
        public const string APIKEY = "doM6wrGqLZqYyd0g4jA86nSmZOVjAXfM28t32XrxhgGUuzgZ9OftzYeNJLvfJMrd";
        public const string APISECRET = "9GkPW2ba27AXOwDXqfykpH4NMEF4iWzANY9MNWfIb9112JSgdoX53UlbyKA3QTGq";
        public static void Main(string[] args)
        {
            GetAppSettingsFile();
            var serviceBusConnectionString = Configuration.GetSection("ServiceBusConnectionString").Value;
            var queueName = Configuration.GetSection("NewSalesOrderQueue").Value;
            queueClient = new QueueClient(serviceBusConnectionString, queueName);
            RegisterOnMessageHandlerAndReceiveMessages();
            Console.ReadKey();
          //  Binance();
        }

         static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
           var newSalesOrder = JsonConvert.DeserializeObject<NewSalesOrder>(Encoding.UTF8.GetString(message.Body));
           Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{newSalesOrder.UserId}");
           Binance(newSalesOrder);
           
           await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


         public static void Binance(NewSalesOrder order){

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                //TODO Mudar a APIKEY
                ApiCredentials = new ApiCredentials(APIKEY, APISECRET),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            using (var client = new BinanceClient())
            {
                var accountInfo = client.GetAccountInfo();
                var orderResult = client.PlaceOrder(order.Coin.Main, OrderSide.Sell, OrderType.StopLossLimit, order.Quantity, price:  order.Limit, stopPrice : order.Stop);
                
                Console.WriteLine(orderResult.Error);
            }

         }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }
    }
}
