using Emul.Models.DataBase.DBModels;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Emulator.Models.Emulator
{
    public class Emulator2
    {
        public List<TH> TradeHistory = new List<TH>();

        public double BalanceUSD { get => balanceUSD; }

        
        private List<Coin_TH> DB = new List<Coin_TH>();

        private DateTime StartTime;
        private DateTime EndTime;

        private double Diff;
        private double PersentDiff;

        private double CheckTime;
        private double BuyTime;
        private double HoldTime;

        private double balanceUSD;
        private double balanceCoin;

        private double feeUSD;
        private double feeCoin;

        private int startIndex;
        private int lastIndex;

        private double totalAmount;
        private double totalTotal;
        private double totalFee;
        private double ABSRate;


        public Emulator2() { }

        public Emulator2(List<Coin_TH> _DB)
        {
            DB = _DB;
        }

        public void Settings(DateTime _StartTime, DateTime _EndTime, double _Diff, double _CheckTime, double _BuyTime, double _HoldTime, double _balance)
        {
            StartTime = _StartTime;
            EndTime = _EndTime;
            Diff = _Diff;
            PersentDiff = Diff / 100;
            CheckTime = _CheckTime;
            BuyTime = _BuyTime;
            HoldTime = _HoldTime;
            balanceUSD = _balance;

            generateStartIndex();
            generateLastIndex();
        }

        private void generateStartIndex()
        {
            for (int i = 0; i < DB.Count; i++)
            {
                if (DB[i].Date > StartTime)
                {
                    startIndex = i;
                    break;
                }
            }
        }

        private void generateLastIndex()
        {
            for(int i = startIndex; i < DB.Count; i++)
            {
                if(DB[i].Date > EndTime)
                {
                    lastIndex = i;
                    break;
                }
            }
        }
        

        public void MakeMoney()
        {
            var SW = new Stopwatch();
            SW.Start();
            //Debug.WriteLine(DateTime.Now + " " + balanceUSD);
            for (int index = startIndex; index < lastIndex; index++)
            {
                if(IsDiff(index) && DB[index].Type == "Sell")
                {
                    index = Buy(index);
                    index = Sell(index);
                }
            }

            SW.Stop();
            var time = SW.ElapsedMilliseconds;
            Debug.WriteLine(time);
        }
        

        private int Buy(int index)
        {
            totalAmount = 0;
            totalFee = 0;
            totalTotal = 0;
            ABSRate = 0;
            int lastIndex = 0;
            DateTimeOffset currentTime = DB[index].Date;
            for (int i = index; i < DB.Count; i++)
            {
                if(currentTime.AddMinutes((int)BuyTime) < DB[i].Date)
                {
                    break;
                }
                else if (DB[i].Type == "Sell")
                {
                    feeUSD = 0.002 * DB[i].Total;
                    totalFee += feeUSD;
                    if (balanceUSD - feeUSD > DB[i].Total)
                    {
                        balanceUSD -= feeUSD;

                        balanceUSD -= DB[i].Total;
                        totalTotal += DB[i].Total;
                        balanceCoin += DB[i].Amount;
                        totalAmount += DB[i].Amount;
                    }
                    else
                    {
                        balanceCoin += ((balanceUSD - feeUSD) / DB[i].Rate);
                        totalAmount += ((balanceUSD - feeUSD) / DB[i].Rate);
                        balanceUSD = 0;
                    }
                    ABSRate = ABSRate == 0 ? DB[i].Rate : (DB[i].Rate + ABSRate) / 2;
                }

                if (balanceUSD == 0)
                    break;
                lastIndex = i;
            }
            TradeHistory.Add(new TH
            {
                Date = currentTime,
                Type = "Buy",
                Rate = ABSRate,
                Amount = totalAmount,
                Total = totalTotal,
                Fee = totalFee,
                Balance = BalanceUSD
            });
            return lastIndex;
        }

        private int Sell(int index)
        {
            totalAmount = 0;
            totalFee = 0;
            totalTotal = 0;
            ABSRate = 0;

            int lastIndex = 0;
            DateTimeOffset currentTime = DB[index].Date;
            for (int i = index; i < DB.Count; i++)
            {
                if (currentTime.AddMinutes((int)HoldTime) < DB[i].Date)
                {
                    if (DB[i].Type == "Buy")
                    {
                        feeCoin = 0.002 * DB[i].Amount;
                        totalFee += feeCoin;
                        if (balanceCoin > DB[i].Amount)
                        {
                            balanceCoin -= feeCoin;

                            balanceCoin -= DB[i].Amount;
                            totalAmount += DB[i].Amount;
                            balanceUSD += DB[i].Total;
                            totalTotal += DB[i].Total;

                        }
                        else
                        {
                            balanceUSD += ((balanceCoin - feeCoin) * DB[i].Rate);
                            totalTotal += ((balanceCoin - feeCoin) * DB[i].Rate);
                            balanceCoin = 0;
                        }
                        ABSRate = ABSRate == 0 ? DB[i].Rate : (DB[i].Rate + ABSRate) / 2;
                    }

                    if (balanceCoin == 0)
                        break;
                }
                lastIndex = i;
            }
            TradeHistory.Add(new TH
            {
                Date = currentTime,
                Type = "Sell",
                Rate = ABSRate,
                Amount = totalAmount,
                Total = totalTotal,
                Fee = totalFee,
                Balance = BalanceUSD
            });
            //Debug.WriteLine(DB[index].Date + " SELL " + balanceUSD);
            return lastIndex;
        }

        private bool IsDiff(int index)
        {

            bool diff = false;

            double cr = CurrentRate(index);
            for (double checkLength = 0.5; checkLength < CheckTime; checkLength += 0.5)
            {
                double ctr = CheckTimeRate(index, checkLength);
                if (ctr < cr - (cr * PersentDiff) &&  cr != 0 && ctr != 0)
                {
                    diff = true;
                    //Debug.WriteLine(ctr);
                    break;
                }
            }

            return diff;
        }

        private double CheckTimeRate(int index, double checkLength)
        {
            double rate = 0;

            int min = (int)checkLength;
            int sec = (((checkLength) - min) * 10) == 5 ? 30 : 0;
            
            DateTimeOffset checkTime = DB[index].Date.AddMinutes(-min).AddSeconds(-sec);

            for(int i = index; i > 0; i--)
            {
                if(DB[i].Date < checkTime && DB[i].Rate != 0)
                {
                    rate = DB[i].Rate;
                    break;
                }
            }
            
            
            return rate;
        }

        private double CurrentRate(int index)
        {
            double rate = 0;

            rate = DB[index].Rate;

            return rate;
        }

    }
}