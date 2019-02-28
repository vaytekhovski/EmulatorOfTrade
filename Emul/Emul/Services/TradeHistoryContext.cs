using System.Data.Entity;
using Emul.Models.EmulatorExam;
using Emulator.Models.DataBase.DBModels;
using QuickType;

namespace Emulator.Models
{
    public class TradeHistoryContext : DbContext
    {
        public DbSet<Coin_TH> TradeHistory { get; set; }
        public DbSet<Examination> Examinations { get; set; }
    }
}