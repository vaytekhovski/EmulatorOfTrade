using QuickType;
using System;
using System.Collections.Generic;

namespace Emulator.Models
{
    public static class DownloadTradeHistory
    {
        private const string FirstPair = "USDT";
        private const string FileName = "returnTradeHistory";
        private static string SecondPair;
        private static int UnixStartDate;
        private static int UnixEndDate;
        private static string SitePath;

        public static List<TradeHistory> CycleDownloadData(DateTime _StartDate, DateTime _EndDate, string _Pair)
        {
            SecondPair = _Pair;

            UnixStartDate = (int)(_StartDate - new DateTime(1970, 1, 1)).TotalSeconds;
            UnixEndDate = (int)(_EndDate - new DateTime(1970, 1, 1)).TotalSeconds;

            SitePath = "https://" + $"poloniex.com/public?command=returnTradeHistory&currencyPair={FirstPair}_{SecondPair}&start={UnixStartDate}&end={UnixEndDate}";

            return TradeHistory.FromJson(JsonToString.GetString(SitePath, FileName));
        }
    }
}