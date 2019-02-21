using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emulator.Models.DataBase.DBModels
{
    public class Coin_TH
    {
        public long GlobalTradeId { get; set; }

        public long TradeId { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Type { get; set; }

        public double Rate { get; set; }

        public double Amount { get; set; }

        public double Total { get; set; }
    }
}