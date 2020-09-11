using System;
using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Domain.Repositories.Readers;
using CryptoOrderApi.Infrastructure.QueueClients;
using ExchangeApi.Domain.Repositories;
using ExchangeApi.Domain.Repositories.Writers;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeApi.Controllers
{
    [Route("/api/stop-limits")]
    public class StopLimitController : Controller
    {
        public IStopLimitWriter StopLimitWriter { get; }

        public IStopLimitReader StopLimitReader { get; }

        public IStopLimitCreatedQueueClient StopLimitCreatedQueueClient { get; }

        public IStopLimitDeletedQueueClient StopLimitDeletedQueueClient { get; }


        public StopLimitController(IStopLimitWriter stopLimitWriter, IStopLimitReader stopLimitReader, IStopLimitCreatedQueueClient stopLimitCreatedQueueClient, IStopLimitDeletedQueueClient stopLimitDeletedQueueClient)
        {
            StopLimitWriter = stopLimitWriter;
            StopLimitReader = stopLimitReader;
            StopLimitCreatedQueueClient = stopLimitCreatedQueueClient;
            StopLimitDeletedQueueClient = stopLimitDeletedQueueClient;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] StopLimit stopLimitModel)
        {
            var stopLimit = await StopLimitWriter.Create(stopLimitModel);

            await StopLimitCreatedQueueClient.Queue(stopLimit);

            return Created(string.Empty, stopLimit);
        }

        [HttpDelete]
        [Route("{stopLimitId}")]
        public async Task<IActionResult> Delete(Guid stopLimitId)
        {
            var stopLimit = await StopLimitReader.Get(stopLimitId);

            if (stopLimit == null) 
            {
                return NotFound();
            }

            var stopLimitDeleted = await StopLimitWriter.Delete(stopLimit);

            await StopLimitDeletedQueueClient.Queue(stopLimitDeleted);

            return Ok(stopLimitDeleted);
        }


        [HttpGet]
        [Route("{stopLimitId}")]
        public async Task<IActionResult> Get(Guid stopLimitId)
        {
            var stopLimit = await StopLimitReader.Get(stopLimitId);

            if (stopLimit == null) 
            {
                return NotFound();
            }
            return Ok(stopLimit);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stopLimits = await StopLimitReader.Get();
            return Ok(stopLimits);
        }
    }
}