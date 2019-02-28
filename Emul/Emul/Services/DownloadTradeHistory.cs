using QuickType;
using System;
using System.Collections.Generic;

namespace Emulator.Models
{
    public static class DownloadTradeHistory
    {
        private static string FirstPair;
        private static string SecondPair;

        static public List<TradeHistory> CycleDownloadData(DateTime _StartDate, DateTime _EndDate, string _Pair)
        {
            FirstPair = "USDT";
            SecondPair = _Pair;
            
            string FileName = "returnTradeHistory";

            int UnixStartDate = (int)(_StartDate - new DateTime(1970, 1, 1)).TotalSeconds;
            int UnixEndDate = (int)(_EndDate - new DateTime(1970, 1, 1)).TotalSeconds;

            string site = "https://" + $"poloniex.com/public?command=returnTradeHistory&currencyPair={FirstPair}_{SecondPair}&start={UnixStartDate}&end={UnixEndDate}";
            

            return TradeHistory.FromJson(JsonToString.GetString(site, FileName));
        }
    }
}