using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Emulator.Models
{
    public class ChartDataDbInitializer : DropCreateDatabaseAlways<TradeContext>
    {
        protected override void Seed(TradeContext context)
        {
            string site = "https://poloniex.com/public?command=returnChartData&currencyPair=BTC_XMR&start=1405699200&end=9999999999&period=14400";
            string FileName = "returnChartData";
            var data1 = QuickType.ChartData.FromJson(QuickType.JsonToString.GetString(site, FileName));
            foreach (var value in data1)
            {
                context.ChartDatas.Add(new QuickType.ChartData
                {
                    Date = value.Date,
                    High = value.High,
                    Low = value.Low,
                    Open = value.Open,
                    Close = value.Close,
                    Volume = value.Volume,
                    QuoteVolume = value.QuoteVolume,
                    WeightedAverage = value.WeightedAverage
                });

            }

            base.Seed(context);
        }
    }

}