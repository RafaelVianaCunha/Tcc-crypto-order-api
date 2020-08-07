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
    public class SaleOrderController : Controller
    {
        public ISaleOrderWriter SaleOrderWriter { get; }

        public ISaleOrderReader SaleOrderReader { get; }


        public SaleOrderController(ISaleOrderWriter saleOrderWriter, ISaleOrderReader saleOrderReader)
        {
           SaleOrderWriter = saleOrderWriter;
           SaleOrderReader = saleOrderReader;

        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] SaleOrder saleOrderModel)
        {
            var saleOrder = await SaleOrderWriter.Create(saleOrderModel);

            return Created(string.Empty, saleOrder);
        }

        [HttpDelete]
        [Route("{saleOrderId}")]
        public async Task<IActionResult> Delete(Guid saleOrderId)
        {
            var saleOrder = await SaleOrderReader.Get(saleOrderId);

            if (saleOrder == null) 
            {
                return NotFound();
            }

            var saleOrderDelete = await SaleOrderWriter.Delete(saleOrder);

            return Ok(saleOrderDelete);
        }


        [HttpGet]
        [Route("{saleOrderId}")]
        public async Task<IActionResult> Get(Guid saleOrderId)
        {
            var saleOrder = await SaleOrderReader.Get(saleOrderId);

            if (saleOrder == null) 
            {
                return NotFound();
            }
            return Ok(saleOrder);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var saleOrders = await SaleOrderReader.Get();
            return Ok(saleOrders);
        }
    }
}