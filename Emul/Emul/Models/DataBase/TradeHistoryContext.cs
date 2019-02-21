using System.Data.Entity;
using Emulator.Models.DataBase.DBModels;
using QuickType;

namespace Emulator.Models
{
    public class TradeHistoryContext : DbContext
    {
        //public DbSet<TradeHistory> Histories { get; set; }

        public DbSet<BTC_TH> BTC_TradeHistory { get; set; }
        public DbSet<XRP_TH> XRP_TradeHistory { get; set; }
        public DbSet<ETH_TH> ETH_TradeHistory { get; set; }

        public DbSet<ChartData> ChartDatas { get; set; }
    }
}