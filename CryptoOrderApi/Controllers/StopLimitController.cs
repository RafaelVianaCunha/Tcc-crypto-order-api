using System;
using System.Threading.Tasks;
using CryptoOrderApi.Domain.Entities;
using CryptoOrderApi.Domain.Repositories.Readers;
using ExchangeApi.Domain.Repositories;
using ExchangeApi.Domain.Repositories.Writers;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeApi.Controllers
{
    [Route("/api/exchanges")]
    public class StopLimitController : Controller
    {
        public IStopLimitWriter StopLimitWriter { get; }

        public IStopLimitReader StopLimitReader { get; }


        public StopLimitController(IStopLimitWriter stopLimitWriter,   IStopLimitReader stopLimitReader)
        {
           StopLimitWriter = stopLimitWriter;
           StopLimitReader = stopLimitReader;

        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] StopLimit stopLimitModel)
        {
            var stopLimit = await StopLimitWriter.Create(stopLimitModel);

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

            var stopLimitDelete = await StopLimitWriter.Delete(stopLimit);

            return Ok(stopLimit);
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