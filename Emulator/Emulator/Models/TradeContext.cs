using System.Data.Entity;
using QuickType;

namespace Emulator.Models
{
    public class TradeContext : DbContext
    {
        public DbSet<TradeHistory> Histories { get; set; }
        public DbSet<ChartData> ChartDatas { get; set; }
    }
}