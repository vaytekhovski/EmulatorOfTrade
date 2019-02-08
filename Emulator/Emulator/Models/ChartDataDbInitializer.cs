using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Emulator.Models
{
    public class ChartDataDbInitializer : DropCreateDatabaseAlways<TradeContext>
    {
        DateTime StartDate, EndDate;
        public ChartDataDbInitializer(DateTime _StartDate, DateTime _EndDate)
        {
            StartDate = _StartDate;
            EndDate = _EndDate;
        }

        protected override void Seed(TradeContext context)
        {

            int UnixStartDate = (int)(StartDate - new DateTime(1970, 1, 1)).TotalSeconds;

            int UnixEndDate = (int)(EndDate - new DateTime(1970, 1, 1)).TotalSeconds;

            string site = "https://"+$"poloniex.com/public?command=returnChartData&currencyPair=BTC_XMR&start={UnixStartDate}&end={UnixEndDate}&period=14400";
            string FileName = "returnChartData";
            var data1 = QuickType.ChartData.FromJson(QuickType.JsonToString.GetString(site, FileName));
            context.ChartDatas.AddRange(data1);

            base.Seed(context);
        }
    }

}