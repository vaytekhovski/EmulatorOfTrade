using System.Data.Entity;

namespace QuickType
{
    public class EmulatorContext : DbContext
    {
        public DbSet<TradeHistory> tradeHistories { get; set; }
    }
}