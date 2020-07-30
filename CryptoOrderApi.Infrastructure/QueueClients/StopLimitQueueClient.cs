using Microsoft.Azure.ServiceBus;
using System;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace CryptoOrderApi.Infrastructure.QueueClients
{
    public class StopLimitQueueQueueClient
    {
        public IQueueClient QueueClient { get; }

        public StopLimitQueueQueueClient(IQueueClient queueClient)
        {
            QueueClient = queueClient;
        }

        public void ConsumeNewStopLimitOrderSale()
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
           var newSalesOrder = JsonConvert.DeserializeObject<NewSalesOrder>(Encoding.UTF8.GetString(message.Body));
           Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{newSalesOrder.UserId}");
           Binance(newSalesOrder);
           
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