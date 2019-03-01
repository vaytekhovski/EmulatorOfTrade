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

        public double BalanceUSD { get; private set; }

        private List<Coin_TH> DB = new List<Coin_TH>();

        private DateTime StartTime;
        private DateTime EndTime;

        private double Diff;
        private double PersentDiff;

        private double CheckTime;
        private double BuyTime;
        private double HoldTime;
        private double balanceCoin;

        private double feeUSD;
        private double feeCoin;
        private const double FEE = 0.002;

        private int startIndex;
        private int lastIndex;

        private double totalAmount;
        private double totalTotal;
        private double totalFee;
        private double ABSRate;


        public Emulator2() { }

        public Emulator2(List<Coin_TH> _DB)
        {
            DB = _DB ?? throw new ArgumentNullException(nameof(_DB));
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
            BalanceUSD = _balance;

            GenerateStartIndex();
            GenerateLastIndex();
        }


        public void MakeMoney()
        {
            var SW = new Stopwatch();
            SW.Start();
            //Debug.WriteLine(DateTime.Now + " " + balanceUSD);

            for (var index = startIndex; index < lastIndex; index++)
            {
                if(IsDiff(index) && DB[index].Type == "Sell")
                {
                    index = Buy(index);
                    index = Sell(index);
                }
            }

            SW.Stop();
            Debug.WriteLine(SW.ElapsedMilliseconds);
            DB.Clear();
        }

        private double CheckTimeRate(int index, double checkLength)
        {
            double rate = 0;

            int min = (int)checkLength;
            int sec = ((checkLength - min) * 10) == 5 ? 30 : 0;

            for (int i = index; i > 0; i--)
            {
                if (DB[i].Rate != 0 && DB[i].Date < DB[index].Date.AddMinutes(-min).AddSeconds(-sec))
                {
                    rate = DB[i].Rate;
                    break;
                }
            }

            return rate;
        }

        private double CurrentRate(int index) => DB[index].Rate;

        private int Buy(int index)
        {
            BreakValues();

            int lastIndex = 0;
            for (int i = index; i < DB.Count; i++)
            {
                if (BalanceUSD == 0)
                    break;

                if (DB[i].Type == "Sell")
                {
                    if (DB[index].Date.AddMinutes((int)BuyTime) > DB[i].Date)
                    {
                        MakePurchaseBUY(i);
                    }
                    else
                        break;
                }

                lastIndex = i;
            }

            AddTH(index, "Buy");
            return lastIndex;
        }

        private int Sell(int index)
        {
            BreakValues();

            int lastIndex = 0;
            for (int i = index; i < DB.Count; i++)
            {
                if (DB[index].Date.AddMinutes((int)HoldTime) < DB[i].Date)
                {
                    if (balanceCoin == 0)
                        break;

                    if (DB[i].Type == "Buy")
                    {
                        MakePurchaseSELL(i);
                    }
                }

                lastIndex = i;
            }


            AddTH(index, "Sell");
            //Debug.WriteLine(DB[index].Date + " SELL " + balanceUSD);
            return lastIndex;
        }

        private bool IsDiff(int index)
        {
            bool diff = false;

            for (var checkLength = 0.5; checkLength < CheckTime; checkLength += 0.5)
            {
                if (CheckTimeRate(index, checkLength) < CurrentRate(index) - (CurrentRate(index) * PersentDiff)
                    && CurrentRate(index) != 0 && CheckTimeRate(index, checkLength) != 0)
                {
                    diff = true;
                    //Debug.WriteLine(ctr);
                    break;
                }
            }

            return diff;
        }

        private void MakePurchaseBUY(int i)
        {
            feeUSD = FEE * DB[i].Total;
            totalFee += feeUSD;
            if (BalanceUSD - feeUSD > DB[i].Total)
            {
                BalanceUSD -= feeUSD;

                BalanceUSD -= DB[i].Total;
                balanceCoin += DB[i].Amount;

                totalTotal += DB[i].Total;
                totalAmount += DB[i].Amount;
            }
            else
            {
                balanceCoin += ((BalanceUSD - feeUSD) / DB[i].Rate);
                BalanceUSD = 0;

                totalAmount += ((BalanceUSD - feeUSD) / DB[i].Rate);
            }
            ABSRate = ABSRate == 0 ? DB[i].Rate : (DB[i].Rate + ABSRate) / 2;
        }

        private void MakePurchaseSELL(int i)
        {
            feeCoin = 0.002 * DB[i].Amount;
            totalFee += feeCoin;
            if (balanceCoin > DB[i].Amount)
            {
                balanceCoin -= feeCoin;

                balanceCoin -= DB[i].Amount;
                totalAmount += DB[i].Amount;
                BalanceUSD += DB[i].Total;
                totalTotal += DB[i].Total;

            }
            else
            {
                BalanceUSD += ((balanceCoin - feeCoin) * DB[i].Rate);
                totalTotal += ((balanceCoin - feeCoin) * DB[i].Rate);
                balanceCoin = 0;
            }
            ABSRate = ABSRate == 0 ? DB[i].Rate : (DB[i].Rate + ABSRate) / 2;
        }

        private void AddTH(int index, string Type)
        {
            TradeHistory.Add(new TH
            {
                Date = DB[index].Date,
                Type = Type,
                Rate = ABSRate,
                Amount = totalAmount,
                Total = totalTotal,
                Fee = totalFee,
                Balance = BalanceUSD
            });
        }

        private void BreakValues()
        {
            totalAmount = 0;
            totalFee = 0;
            totalTotal = 0;
            ABSRate = 0;
        }
        
        private void GenerateStartIndex()
        {
            for (var i = 0; i < DB.Count; i++)
            {
                if (DB[i].Date > StartTime)
                {
                    startIndex = i;
                    break;
                }
            }
        }

        private void GenerateLastIndex()
        {
            for (var i = startIndex; i < DB.Count; i++)
            {
                if (DB[i].Date > EndTime)
                {
                    lastIndex = i;
                    break;
                }
            }
        }

    }
}