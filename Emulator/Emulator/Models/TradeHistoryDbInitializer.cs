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
            context.Histories.AddRange(CycleDownloadData());
            base.Seed(context);
        }

        List<QuickType.TradeHistory> CycleDownloadData()
        {
            DateTime start, end;
            start = EndDate;
            end = EndDate;
            List<QuickType.TradeHistory> data = new List<QuickType.TradeHistory>();
            string FileName = "returnTradeHistory";
            

            int i = 0;
            do
            {
                end = EndDate.AddDays(-i);

                if (start.AddDays(-10).Date < StartDate)
                    start = StartDate;
                else
                    start = end.AddDays(-10);


                int UnixStartDate = (int)(start - new DateTime(1970, 1, 1)).TotalSeconds;
                int UnixEndDate = (int)(end - new DateTime(1970, 1, 1)).TotalSeconds;

                string site = "https://" + $"poloniex.com/public?command=returnTradeHistory&currencyPair={FirstPair}_{SecondPair}&start={UnixStartDate}&end={UnixEndDate}";
                data.AddRange(QuickType.TradeHistory.FromJson(QuickType.JsonToString.GetString(site, FileName)));

                i += 10;
            } while (start != StartDate);


            return data;
        }
    }
}