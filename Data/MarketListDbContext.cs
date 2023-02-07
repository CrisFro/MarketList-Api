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

        // DbSet 

        public DbSet<Market> Markets { get; set; }


        public Stream ExportCsv()
        {
            var csv = new StringBuilder();

            var markets = Markets;

            var columns = "TIPO DE PRODUTO;DESCRIÇÃO";
            csv.AppendLine(columns);

            foreach (var item in markets)
            {
                csv.AppendLine($"{item.ProductType.ToString()};" +
                    $"{item.ProductDescription}");
            }

            var randomString = Guid.NewGuid().ToString();

            string filePath = $"C:\\teste\\{randomString}.csv";

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);

            Stream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return file;
        }

        
    }


}
