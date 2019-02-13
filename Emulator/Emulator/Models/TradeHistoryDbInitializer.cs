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
            context.Histories.AddRange(DownloadTradeHistory.CycleDownloadData(StartDate,EndDate,FirstPair,SecondPair));
            base.Seed(context);
        }

        public void Update(TradeContext context)
        {
            
            context.Histories.AddRange(DownloadTradeHistory.CycleDownloadData(StartDate, EndDate, FirstPair, SecondPair));
        }

        
    }
}