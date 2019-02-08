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
        string FirstPair, SecondPair;
        public TradeHistoryDbInitializer(DateTime _StartDate, DateTime _EndDate, string fPair, string sPair)
        {
            StartDate = _StartDate;
            EndDate = _EndDate;
            FirstPair = fPair;
            SecondPair = sPair;
        }
        protected override void Seed(TradeContext context)
        {
            int UnixStartDate = (int)(StartDate - new DateTime(1970, 1, 1)).TotalSeconds;

            int UnixEndDate = (int)(EndDate - new DateTime(1970, 1, 1)).TotalSeconds;

            string site = "https://" + $"poloniex.com/public?command=returnTradeHistory&currencyPair={FirstPair}_{SecondPair}&start={UnixStartDate}&end={UnixEndDate}";
            string FileName = "returnTradeHistory";
            var data = QuickType.TradeHistory.FromJson(QuickType.JsonToString.GetString(site, FileName));

            context.Histories.AddRange(data);
            base.Seed(context);
        }
    }
}