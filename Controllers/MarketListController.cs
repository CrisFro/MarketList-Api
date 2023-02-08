using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarketList_Api.Data;
using MarketList_Api.Models;

namespace MarketList_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketListController : ControllerBase
    {
        private readonly MarketListDbContext _context;

        public MarketListController(MarketListDbContext context)
        {
            _context = context;
        }

        // GET: api/MarketList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Market>>> GetMarkets()
        {
            return await _context.Markets.ToListAsync();
        }

        // GET: api/MarketList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Market>> GetMarket(Guid id)
        {
            var market = await _context.Markets.FindAsync(id);

            if (market == null)
            {
                return NotFound();
            }

            return market;
        }

        // PUT: api/MarketList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarket(Guid id, Market market)
        {
            if (id != market.Id)
            {
                return BadRequest();
            }

            _context.Entry(market).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MarketList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Market>> PostMarket(Market market)
        {
            _context.Markets.Add(market);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMarket", new { id = market.Id }, market);
        }

        // DELETE: api/MarketList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarket(Guid id)
        {
            var market = await _context.Markets.FindAsync(id);
            if (market == null)
            {
                return NotFound();
            }

            _context.Markets.Remove(market);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MarketExists(Guid id)
        {
            return _context.Markets.Any(e => e.Id == id);
        }

        [HttpPost("export")]
        public IActionResult Export()
        {
            var csvFile = _context.ExportCsv();

            return File(csvFile, "application/csv", "arquivo.csv");

        }
    }
}
