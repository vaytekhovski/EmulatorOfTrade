using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Emulator.Models
{
    public class TradeHistoryDbInitializer : DropCreateDatabaseAlways<TradeContext>
    {
        DateTime StartDate, EndDate;
        public TradeHistoryDbInitializer(DateTime _StartDate, DateTime _EndDate)
        {
            StartDate = _StartDate;
            EndDate = _EndDate;
        }

        protected override void Seed(TradeContext context)
        {
            int UnixStartDate = (int)(StartDate - new DateTime(1970, 1, 1)).TotalSeconds;

            int UnixEndDate = (int)(EndDate - new DateTime(1970, 1, 1)).TotalSeconds;

            string site = "https://" + $"poloniex.com/public?command=returnTradeHistory&currencyPair=BTC_ETH&start={UnixStartDate}&end={UnixEndDate}";
            string FileName = "returnTradeHistory";
            var data = QuickType.TradeHistory.FromJson(QuickType.JsonToString.GetString(site, FileName));
            foreach (var value in data)
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