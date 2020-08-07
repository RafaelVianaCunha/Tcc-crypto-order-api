using CryptoOrderApi.Domain.Repositories.Readers;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Domain.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using ExchangeApi.Domain.Repositories.Writers;

namespace CryptoOrderApi.Domain.Services
{
    public class SaleOrderProcessor
    {
        public IStopLimitReader StopLimitReader { get; }

        public ISaleOrderQueueClient SaleOrderQueueClient { get; }

        public IBinanceClient BinanceClient { get; }

        public ISaleOrderWriter SaleOrderWriter { get; }

        public SaleOrderProcessor(
            IStopLimitReader stopLimitReader,
            ISaleOrderQueueClient saleOrderQueueClient,
            IBinanceClient binanceClient,
            ISaleOrderWriter saleOrderWriter)
        {
            StopLimitReader = stopLimitReader;
            SaleOrderQueueClient = saleOrderQueueClient;
            BinanceClient = binanceClient;
            SaleOrderWriter = saleOrderWriter;
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

            await BinanceClient.PlaceOrder(saleOrder);

            await SaleOrderWriter.Create(saleOrder);

            await SaleOrderQueueClient.Queue(saleOrder);

            return saleOrder;
        }
    }
}