using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Emulator.Models;

namespace Emulator.Models
{
    public class TradeHistoryDbInitializer : DropCreateDatabaseAlways<TradeContext>
    {
        DateTime StartDate, EndDate;
        string Pair;
        public TradeHistoryDbInitializer(DateTime _StartDate, DateTime _EndDate, string _Pair)
        {
            StartDate = _StartDate;
            EndDate = _EndDate;
            Pair = _Pair;
        }

        public TradeHistoryDbInitializer()
        {
            StartDate = new DateTime(1999,01,01,0,0,0);
            EndDate = new DateTime(1999, 01, 01, 0, 0, 0);
        }

        protected override void Seed(TradeContext context)
        {
            context.Histories.AddRange(DownloadTradeHistory.CycleDownloadData(StartDate,EndDate,Pair));
            base.Seed(context);
        }
        

        
    }
}