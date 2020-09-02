using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoOrderApi.Domain.Clients;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Domain.ValueObjects;

namespace CryptoOrderApi.Infrastructure.Clients
{
    public class BinenceClient : IBinanceClient
    {
        private const String SYMBOL = "BTCUSD";
        public void PlaceOrder(ExchangeCredentials exchangeCredentials, SaleOrder saleOrder)
        {
              BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(exchangeCredentials.ApiKey, exchangeCredentials.ApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            using (var client = new BinanceClient())
            {
                var accountInfo = client.GetAccountInfo();
                var orderResult = client.PlaceOrder(SYMBOL, OrderSide.Sell, OrderType.StopLossLimit, 
                                                    saleOrder.StopLimit.Quantity, 
                                                    price:  saleOrder.StopLimit.Limit, stopPrice : saleOrder.StopLimit.Stop);
            }
        }
    }
}