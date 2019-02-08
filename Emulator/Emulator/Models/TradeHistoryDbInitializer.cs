using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Emulator.Models
{
    public class TradeHistoryDbInitializer : DropCreateDatabaseAlways<TradeContext>
    {
        protected override void Seed(TradeContext context)
        {
            string site = "https://poloniex.com/public?command=returnTradeHistory&currencyPair=BTC_ETH";
            string FileName = "returnTradeHistory";
            var data = QuickType.TradeHistory.FromJson(QuickType.JsonToString.GetString(site, FileName));
            
            foreach(var value in data)
            {
                context.Histories.Add(new QuickType.TradeHistory
                {
                    GlobalTradeId = value.GlobalTradeId,
                    TradeId = value.TradeId,
                    Date = value.Date,
                    Type = value.Type,
                    Rate = value.Rate,
                    Amount = value.Amount,
                    Total = value.Total
                });

            }
            
            base.Seed(context);
        }
    }
}