using CryptoOrderApi.Domain.Clients;
using CryptoOrderApi.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using CryptoOrderApi.Infrastructure.Messages;

namespace CryptoOrderApi.Infrastructure.QueueClients 
{
    public class StopLimitCreatedQueueClient : IStopLimitCreatedQueueClient
    {
        public IQueueClient QueueClient { get; }

        public StopLimitCreatedQueueClient(IQueueClient queueClient)
        {
            QueueClient = queueClient;
        }

        public async Task Queue(StopLimit stopLimit)
        {
            var stopLimitCreated = new StopLimitCreated() 
            {
                Id = stopLimit.Id,
                UserID = stopLimit.UserID,
                Stop = stopLimit.Stop,
                Limit = stopLimit.Limit,
                Quantity = stopLimit.Quantity,
                CreatedAt = stopLimit.CreatedAt
            };

            var messageBody = JsonConvert.SerializeObject(stopLimit);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            
            await QueueClient.SendAsync(message);
        }
    }
}
