using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Diagnostics;
using Emulator.Models.DataBase.DBModels;

namespace Emulator.Models.Emulator
{
    public class Emulator
    {
        public Emulator() { }

        //List<QuickType.TradeHistory> orderedHistories = OwnDataBase.database.BTC_TradeHistory.OrderBy(history => history.Date).ToList();

        List<BTC_TH> orderedHistories = OwnDataBase.database.BTC_TradeHistory.OrderBy(history => history.Date).ToList();

        string Coin;
        
        DateTime StartTime, EndTime;
        
        double Diff;
        
        double CheckTime, BuyTime, HoldTime;

        double balanceUSD, balanceCoin;
        double feeUSD = 0, feeCoin = 0;
        

        public Emulator(string _Coin, DateTime _StartTime, DateTime _EndTime, double _Diff, double _CheckTime, double _BuyTime, double _HoldTime, double _balance)
        {
            Coin = _Coin;
            StartTime = _StartTime;
            EndTime = _EndTime;
            Diff = _Diff;
            CheckTime = _CheckTime;
            BuyTime = _BuyTime;
            HoldTime = _HoldTime;
            balanceUSD = _balance;

            StartTime = new DateTime(_StartTime.Year, _StartTime.Month, _StartTime.Day, 0, 01, 00);
            
        }


        public void MakeMoney()
        {
            DateTimeOffset lastSellTime = StartTime;
            Debug.WriteLine(DateTime.Now + " " + balanceUSD);
            foreach (var item in orderedHistories)
            {
                // если текущий момент позже времени последней продажи
                if (item.Date > lastSellTime)
                {
                    for (double tempCheckTime = CheckTime; tempCheckTime > 0; tempCheckTime -= 0.5)
                    {
                        bool isDiff = ReturnCourseValue(item.Date, "Sell") / ReturnCourseValue(item.Date.AddMinutes(-tempCheckTime), "Sell") >= Diff;

                        if (isDiff)
                        {
                            Buy(item.Date);
                            Sell(item.Date.AddMinutes(BuyTime));

                            lastSellTime = item.Date.AddMinutes(HoldTime + BuyTime);

                            break;
                        }
                    }
                }
            }
            Debug.WriteLine(DateTime.Now + " " + balanceUSD);
        }
        

        private double ReturnCourseValue(DateTimeOffset currentTime, string Type)
        {
            double value = 0;
            foreach (var data in orderedHistories)
            {
                if (Type == "Sell" && data.Type.ToString() == Type)
                {
                    if (data.Date == currentTime)
                    {
                        value = data.Rate;
                        break;
                    }
                }
                else if (Type == "Buy" && data.Type.ToString() == Type)
                {
                    foreach (var item in orderedHistories)
                    {
                        if (item.Date > currentTime)
                        {
                            value = item.Rate;
                            break;
                        }
                    }
                    break;
                }
            }

            return value;
        }
        
        private void MakePurchase(DateTimeOffset currentTime, string Type)
        {
            foreach (var data in orderedHistories)
                if (Type == "Sell" && data.Type.ToString() == Type)
                {
                    if (data.Date == currentTime)
                    {

                        feeUSD = 0.002 * data.Total;
                        if (balanceUSD - feeUSD > data.Total)
                        {
                            balanceUSD -= feeUSD;

                            balanceUSD -= data.Total;
                            balanceCoin += data.Amount;
                        }
                        else
                        {
                            balanceCoin += ((balanceUSD - feeUSD) / data.Rate);
                            balanceUSD = 0;
                        }
                        break;
                    }
                }
                else if (Type == "Buy" && data.Type.ToString() == Type)
                {
                    if (data.Date > currentTime)
                    {
                        feeCoin = 0.002 * data.Amount;
                        if (balanceCoin > data.Amount)
                        {
                            balanceCoin -= feeCoin;

                            balanceCoin -= data.Amount;
                            balanceUSD += data.Total;
                            
                        }
                        else
                        {
                            balanceUSD += ((balanceCoin - feeCoin) * data.Rate);
                            balanceCoin = 0;
                        }
                        break;
                    }
                }
        }


        private void Buy(DateTimeOffset currentTime)
        {
            foreach (var data in orderedHistories)
                if (data.Date >= currentTime && data.Date <= currentTime.AddMinutes(BuyTime))
                    MakePurchase(data.Date, "Sell");
                else if (data.Date >= currentTime.AddMinutes(BuyTime))
                    break;

            //Debug.WriteLine(currentTime + "  Buy" + " USD:" + balanceUSD + " Coin:" + balanceCoin);
        }


        private void Sell(DateTimeOffset currentTime)
        {
            foreach (var data in orderedHistories)
                if (data.Date >= currentTime.AddMinutes(HoldTime))
                    if(balanceCoin > 0)
                        MakePurchase(data.Date, "Buy");
                    else
                        break;

           // Debug.WriteLine(currentTime.AddMinutes(HoldTime) + " Sell" + " USD:" + balanceUSD + " Coin:" + balanceCoin);
        }
        
    }
}