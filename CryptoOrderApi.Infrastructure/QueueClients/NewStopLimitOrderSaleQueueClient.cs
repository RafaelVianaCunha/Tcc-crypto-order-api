using Microsoft.Azure.ServiceBus;
using System;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using CryptoOrderApi.Infrastructure.Messages;
using CryptoOrderApi.Domain.Repositories.Readers;
using CryptoOrderApi.Domain.Services;

namespace CryptoOrderApi.Infrastructure.QueueClients
{
    public class NewStopLimitOrderSaleQueueClient : INewStopLimitOrderSaleQueueClient
    {
        public IQueueClient QueueClient { get; }

        public IStopLimitReader StopLimitReader { get; }

        public ISaleOrderProcessor SaleOrderProcessor { get; }

        public NewStopLimitOrderSaleQueueClient(IQueueClient queueClient, IStopLimitReader stopLimitReader, ISaleOrderProcessor saleOrderProcessor)
        {
            StopLimitReader = stopLimitReader;
            QueueClient = queueClient;
            SaleOrderProcessor = saleOrderProcessor;
        }

        public void Consume()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            // Register the function that will process messages
            QueueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var newSaleOrder = JsonConvert.DeserializeObject<NewStopLimitOrderSale>(Encoding.UTF8.GetString(message.Body));
            Console.WriteLine("vamos vender");
        
            await SaleOrderProcessor.Sell(newSaleOrder.StopLimitId);

            await QueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }        
    }
}