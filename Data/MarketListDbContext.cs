using MarketList_Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text;

namespace MarketList_Api.Data
{
    public class MarketListDbContext : DbContext
    {
        public MarketListDbContext(DbContextOptions<MarketListDbContext> options) : base(options) { }

        public DbSet<Market> Markets { get; set; }

    }


}
