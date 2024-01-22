﻿using MarketList_Api.Models;
using MarketList_Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MarketList_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MarketListController : ControllerBase
    {
        private readonly IMarketInterface _marketInterface;

        public MarketListController(IMarketInterface marketInterface)
        {
            _marketInterface = marketInterface;
        }

        // GET: api/MarketList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Market>>> GetMarkets()
        {
            var markets = await _marketInterface.GetAllMarketsAsync();
            return Ok(markets);
        }

        // GET: api/MarketList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Market>> GetMarket(Guid id)
        {
            var market = await _marketInterface.GetMarketByIdAsync(id);

            if (market == null)
            {
                return NotFound();
            }

            return Ok(market);
        }

        // PUT: api/MarketList/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarket(Guid id, Market market)
        {
            if (id != market.Id)
            {
                return BadRequest();
            }

            try
            {
                await _marketInterface.UpdateMarketAsync(market);
                return NoContent();
            }
            catch (MarketNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST: api/MarketList
        [HttpPost]
        public async Task<ActionResult<Market>> PostMarket(Market market)
        {
            await _marketInterface.AddMarketAsync(market);
            return CreatedAtAction(nameof(GetMarket), new { id = market.Id }, market);
        }

        // DELETE: api/MarketList/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarket(Guid id)
        {
            try
            {
                await _marketInterface.DeleteMarketAsync(id);
                return NoContent();
            }
            catch (MarketNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPost("export")]
        public async Task<IActionResult> Export()
        {
            try
            {
                var csvFileStream = await _marketInterface.ExportCsv();

                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "arquivo.csv",
                };

                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                return File(csvFileStream, "application/csv");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}