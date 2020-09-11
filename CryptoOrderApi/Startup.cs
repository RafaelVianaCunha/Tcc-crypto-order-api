using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Interfaces;
using CryptoOrderApi.Domain.Clients;
using CryptoOrderApi.Domain.Repositories.Readers;
using CryptoOrderApi.Domain.Services;
using CryptoOrderApi.Infrastructure;
using CryptoOrderApi.Infrastructure.Clients;
using CryptoOrderApi.Infrastructure.QueueClients;
using CryptoOrderApi.Infrastructure.Repositories.Readers;
using CryptoOrderApi.Infrastructure.Repositories.Writers;
using ExchangeApi.Domain.Repositories.Writers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace CryptoOrderApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        SimpleInjector.Container container = new SimpleInjector.Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSimpleInjector(container, options => {
                options
                    .AddAspNetCore()
                    .AddControllerActivation()
                    .AddViewComponentActivation();
            });

            InitializeContainer();
        }

        private void InitializeContainer()
        {
            var serviceBusConnectionString = Configuration.GetSection("ServiceBusConnectionString").Value;
            
            var newStopLimitOrderSaleQueueName = Configuration.GetSection("NewStopLimitOrderSaleQueue").Value;
            var newStopLimitOrderSaleQueueClient = new QueueClient(serviceBusConnectionString, newStopLimitOrderSaleQueueName);

            var saleOrderExecutedQueueName = Configuration.GetSection("SaleOrderExecutedQueue").Value;
            var saleOrderExecutedQueueClient = new QueueClient(serviceBusConnectionString, saleOrderExecutedQueueName);

            var stopLimitCreatedQueueName = Configuration.GetSection("StopLimitCreatedQueue").Value;
            var stopLimitCreatedQueueClient = new QueueClient(serviceBusConnectionString, stopLimitCreatedQueueName);

            var stopLimitDeletedQueueName = Configuration.GetSection("StopLimitDeletedQueue").Value;
            var stopLimitDeletedQueueClient = new QueueClient(serviceBusConnectionString, stopLimitDeletedQueueName);

            container.Register<Domain.Clients.IBinanceClient, BinenceClient>();
            container.Register<IExchangeCredentialsClient>(() => {
                var exchangeUrlApi = Configuration.GetSection("ExchangeApiUrl").Value;

                return new ExchangeCredentialsClient(new System.Net.Http.HttpClient(), exchangeUrlApi);
            });

            container.Register<ISaleOrderProcessor, SaleOrderProcessor>();

            container.Register<StopLimitDbContext>(() => {
                var connectionString = Configuration.GetSection("CryptoOrderDb").Value.ToString();
            
                var optionsBuilder = new DbContextOptionsBuilder<StopLimitDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                return new StopLimitDbContext(optionsBuilder.Options);
            }, Lifestyle.Scoped);
                
            container.Register<IStopLimitReader, StopLimitReader>();
            container.Register<ISaleOrderReader, SaleOrderReader>();

            container.Register<IStopLimitWriter, StopLimitWriter>();
            container.Register<ISaleOrderWriter, SaleOrderWriter>();
            
            container.Register<INewStopLimitOrderSaleQueueClient>(() => {
                var stopLimitReader = container.GetInstance<IStopLimitReader>();
                var saleOrderProcessor = container.GetInstance<ISaleOrderProcessor>();
                return new NewStopLimitOrderSaleQueueClient(newStopLimitOrderSaleQueueClient, stopLimitReader, saleOrderProcessor);
            }, Lifestyle.Singleton);

            container.Register<ISaleOrderExecutedQueueClient>(() => {
                return new SaleOrderExecutedQueueClient(saleOrderExecutedQueueClient);
            });

            container.Register<IStopLimitCreatedQueueClient>(() => {
                return new StopLimitCreatedQueueClient(stopLimitCreatedQueueClient);
            });

            container.Register<IStopLimitDeletedQueueClient>(() => {
                return new StopLimitDeletedQueueClient(stopLimitDeletedQueueClient);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSimpleInjector(container);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            container.Verify();
            
            InitializeQueues();
        }

        private void InitializeQueues()
        {
            var newStopLimitOrderSaleQueueClient = container.GetInstance<INewStopLimitOrderSaleQueueClient>();
            newStopLimitOrderSaleQueueClient.Consume();
        }
    }
}
