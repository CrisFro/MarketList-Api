using MarketList_Api.Data;
using MarketList_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MarketList_Api.Services
{
    public class MarketService : IMarketInterface
    {
        private readonly MarketListDbContext _context;

        public MarketService(MarketListDbContext context)
        {
            _context = context;
        }

        public async Task<List<Market>> GetAllMarketsAsync()
        {
            return await _context.Markets.ToListAsync();
        }

        public async Task<Market> GetMarketByIdAsync(Guid id)
        {
            return await _context.Markets.FindAsync(id);
        }

        public async Task AddMarketAsync(Market market)
        {
            _context.Markets.Add(market);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMarketAsync(Market market)
        {
            var existingMarket = await _context.Markets.FindAsync(market.Id);

            if (existingMarket == null)
            {
                throw new MarketNotFoundException();
            }

            existingMarket.ProductType = market.ProductType;
            existingMarket.ProductDescription = market.ProductDescription;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMarketAsync(Guid id)
        {
            var market = await _context.Markets.FindAsync(id);

            if (market == null)
            {
                throw new MarketNotFoundException();
            }

            _context.Markets.Remove(market);
            await _context.SaveChangesAsync();
        }

        public async Task<Stream> ExportCsv()
        {
            var markets = await _context.Markets.ToListAsync();

            var csv = new StringBuilder();
            var columns = "TIPO DE PRODUTO;DESCRIÇÃO";
            csv.AppendLine(columns);

            foreach (var item in markets)
            {
                csv.AppendLine($"{item.ProductType};{item.ProductDescription}");
            }

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(csv.ToString()));

            memoryStream.Position = 0;

            return memoryStream;
        }
    }

    public class MarketNotFoundException : Exception
    {
        public MarketNotFoundException() : base("Item não encontrado.")
        {
        }
    }
}
