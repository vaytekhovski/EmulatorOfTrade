using System.Collections.Generic;
using System.Data.Entity;
using Emul.Models.Account;
using Emul.Models.DataBase.DBModels;
using Emul.Models.EmulatorExam;
using Emulator.Models.DataBase.DBModels;

namespace Emulator.Models
{
    public class TradeHistoryContext : DbContext
    {
        public DbSet<Coin_TH> TradeHistory { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<TH> TradeHistories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}