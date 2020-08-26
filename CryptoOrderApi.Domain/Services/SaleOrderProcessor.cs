using CryptoOrderApi.Domain.Repositories.Readers;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Domain.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using ExchangeApi.Domain.Repositories.Writers;

namespace CryptoOrderApi.Domain.Services
{
    public class SaleOrderProcessor : ISaleOrderProcessor
    {
        public IStopLimitReader StopLimitReader { get; }

        public ISaleOrderExecutedQueueClient SaleOrderExecutedQueueClient { get; }

        public IBinanceClient BinanceClient { get; }

        public ISaleOrderWriter SaleOrderWriter { get; }

        public IExchangeCredentialsClient ExchangeCredentialsClient { get; }

        public SaleOrderProcessor(
            IStopLimitReader stopLimitReader,
            ISaleOrderExecutedQueueClient saleOrderExecutedQueueClient,
            IBinanceClient binanceClient,
            ISaleOrderWriter saleOrderWriter,
            IExchangeCredentialsClient exchangeCredentialsClient)
        {
            StopLimitReader = stopLimitReader;
            SaleOrderExecutedQueueClient = saleOrderExecutedQueueClient;
            BinanceClient = binanceClient;
            SaleOrderWriter = saleOrderWriter;
            ExchangeCredentialsClient = exchangeCredentialsClient;
        }

        public async Task<SaleOrder> Sell(Guid stopLimitId)
        {
            var stopLimit = await StopLimitReader.Get(stopLimitId);

            if (stopLimit == null)
                throw new Exception("StopLimit not found");  

            var saleOrder = new SaleOrder
            {
                Id = Guid.NewGuid(),
                ExecutedAt = DateTime.UtcNow,
                StopLimit = stopLimit,
                StopLimitId = stopLimitId
            };

            var exchangeCredentials = await ExchangeCredentialsClient.Get(stopLimit.UserID);

            BinanceClient.PlaceOrder(exchangeCredentials, saleOrder);

            await SaleOrderWriter.Create(saleOrder);

            await SaleOrderExecutedQueueClient.Queue(saleOrder);

            return saleOrder;
        }
    }
}