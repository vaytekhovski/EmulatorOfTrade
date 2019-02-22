using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Emulator.Models
{
    public static class DownloadTradeHistory
    {
        private static string FirstPair, SecondPair;

        static public List<QuickType.TradeHistory> CycleDownloadData(DateTime _StartDate, DateTime _EndDate, string _Pair)
        {
            FirstPair = "USDT";
            SecondPair = _Pair;
            
            string FileName = "returnTradeHistory";

            int UnixStartDate = (int)(_StartDate - new DateTime(1970, 1, 1)).TotalSeconds;
            int UnixEndDate = (int)(_EndDate - new DateTime(1970, 1, 1)).TotalSeconds;

            string site = "https://" + $"poloniex.com/public?command=returnTradeHistory&currencyPair={FirstPair}_{SecondPair}&start={UnixStartDate}&end={UnixEndDate}";


            

            return QuickType.TradeHistory.FromJson(QuickType.JsonToString.GetString(site, FileName));
        }
    }
}