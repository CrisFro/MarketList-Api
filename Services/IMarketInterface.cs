using MarketList_Api.Models;

namespace MarketList_Api.Services
{
    public interface IMarketInterface
    {
        Task<List<Market>> GetAllMarketsAsync();
        Task<Market> GetMarketByIdAsync(Guid id);
        Task AddMarketAsync(Market market);
        Task UpdateMarketAsync(Market market);
        Task DeleteMarketAsync(Guid id);
        Task<Stream> ExportCsv();
    }
}
