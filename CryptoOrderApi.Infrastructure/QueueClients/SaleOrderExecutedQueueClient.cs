using CryptoOrderApi.Domain.Clients;
using CryptoOrderApi.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace CryptoOrderApi.Infrastructure.QueueClients 
{
    public class SaleOrderExecutedQueueClient : ISaleOrderExecutedQueueClient
    {
        public IQueueClient QueueClient { get; }

        public SaleOrderExecutedQueueClient(IQueueClient queueClient)
        {
            QueueClient = queueClient;
        }

        public async Task Queue(SaleOrder saleOrder)
        {
            var messageBody = JsonConvert.SerializeObject(saleOrder);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            
            await QueueClient.SendAsync(message);
        }
    }
}
