using Emulator.Models.DataBase.DBModels;
using System.Collections.Generic;
using System.Diagnostics;

namespace Emul.Models.Emulator
{
    public static class Parser
    {
        static public List<Coin_TH> DB = new List<Coin_TH>();

        static public void Parse(List<BTC_TH> list)
        {
            Debug.WriteLine($"Parse BTC trade history started");
            DB.Clear();
            foreach (var item in list)
            {
                DB.Add(new Coin_TH
                {
                    GlobalTradeId = item.GlobalTradeId,
                    TradeId = item.TradeId,
                    Date = item.Date,
                    Type = item.Type.ToString(),
                    Rate = item.Rate,
                    Amount = item.Amount,
                    Total = item.Total
                });
            }
            Debug.WriteLine($"Parse BTC trade history ended");
        }

        static public void Parse(List<ETH_TH> list)
        {
            Debug.WriteLine($"Parse ETH trade history started");
            DB.Clear();
            foreach (var item in list)
            {
                DB.Add(new Coin_TH
                {
                    GlobalTradeId = item.GlobalTradeId,
                    TradeId = item.TradeId,
                    Date = item.Date,
                    Type = item.Type.ToString(),
                    Rate = item.Rate,
                    Amount = item.Amount,
                    Total = item.Total
                });
            }
            Debug.WriteLine($"Parse ETH trade history ended");
        }

        static public void Parse(List<XRP_TH> list)
        {
            Debug.WriteLine($"Parse XRP trade history started");
            DB.Clear();
            foreach (var item in list)
            {
                DB.Add(new Coin_TH
                {
                    GlobalTradeId = item.GlobalTradeId,
                    TradeId = item.TradeId,
                    Date = item.Date,
                    Type = item.Type.ToString(),
                    Rate = item.Rate,
                    Amount = item.Amount,
                    Total = item.Total
                });
            }
            Debug.WriteLine($"Parse XRP trade history ended");
        }
    }
}