
using System;

namespace Emulator.Models.DataBase.DBModels
{
    public class Coin_TH
    {
        public long Id { get; set; }
        public string CurrencyName { get; set; }
        public long GlobalTradeId { get; set; }
        public long TradeId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Type { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double Total { get; set; }
    }
}