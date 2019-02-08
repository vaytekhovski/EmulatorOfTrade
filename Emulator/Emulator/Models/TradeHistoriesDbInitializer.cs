using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Emulator.Models;
using QuickType;

namespace Emulator.Models.DataBase
{
    public class EmulatorDBInitializer : DropCreateDatabaseAlways<EmulatorContext>
    {

        protected override void Seed(EmulatorContext db)
        {
            string site = "https://poloniex.com/public?command=returnTradeHistory&currencyPair=BTC_ETH";
            string FileName = "returnTradeHistory";
            var data = TradeHistory.FromJson(JsonToString.GetString(site, FileName));


            foreach(TradeHistory value in data)
            {
                db.tradeHistories.Add(new TradeHistory
                {
                    GlobalTradeId = value.GlobalTradeId,
                    TradeId = value.TradeId,
                    TradeDate = value.TradeDate,
                    TradeType = value.TradeType,
                    Amount = value.Amount,
                    Rate = value.Rate,
                    Total = value.Total
                });
                base.Seed(db);
            }
        }
    }
}