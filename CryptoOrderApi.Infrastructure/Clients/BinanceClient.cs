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
        private const String SYMBOL = "BTCUSDT";
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
                Console.WriteLine("Ordem de Stop Executada");
                //var accountInfo = client.GetAccountInfo();
                var orderResult = client.PlaceOrder(SYMBOL, OrderSide.Sell, OrderType.Market, 
                                                    saleOrder.StopLimit.Quantity) ;
            }
        }
    }
}