namespace CryptoOrderApi.Infrastructure.QueueClients
{
    public interface INewStopLimitOrderSaleQueueClient
    {
        void Consume();
    }
}
