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
        

        private List<Coin_TH> DB = new List<Coin_TH>();

        private DateTime StartTime;
        private DateTime EndTime;

        private double Diff;
        private double PersentDiff;

        private double CheckTime;
        private double BuyTime;
        private double HoldTime;
        private double balanceCoin;
        private double balanceUSD;

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
            balanceUSD = _balance;

            GenerateStartIndex();
            GenerateLastIndex();
        }


        public void MakeMoney()
        {
            for (var index = startIndex; index < lastIndex; index++)
            {
                if(IsDiff(index) && DB[index].Type == "Sell")
                {
                    index = Buy(index);
                    index = Sell(index);
                }
            }


        }

        private bool IsDiff(int index)
        {
            bool diff = false;

            double currentRate = CurrentRate(index);
            double checkTimeRate = 0;

            for (var checkLength = 0.5; checkLength < CheckTime; checkLength += 0.5)
            {
                checkTimeRate = CheckTimeRate(index, checkLength);

                if (checkTimeRate < currentRate - (currentRate * PersentDiff) && currentRate != 0 && checkTimeRate != 0)
                {
                    diff = true;
                    break;
                }
            }
            

            return diff;
        }

        private double CheckTimeRate(int index, double checkLength)
        {
            double rate = 0;

            DateTimeOffset checkDate = DB[index].Date.AddMinutes(-checkLength);

            for (int i = index; i > 0; i--)
            {
                if (DB[i].Rate != 0 && DB[i].Date < checkDate)
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
            int i = 0;
            for (i = index; i < DB.Count; i++)
            {
                if (balanceUSD == 0)
                    break;

                if (DB[i].Type == "Sell" && DB[index].Date.AddMinutes(BuyTime) > DB[i].Date)
                {
                    MakePurchaseBUY(i);
                }
                else
                    break;

            }

            lastIndex = i;
            AddTH(index, "Buy");
            return lastIndex;
        }

        private int Sell(int index)
        {
            BreakValues();

            int lastIndex = 0;
            int i = 0;
            for (i = index; i < DB.Count; i++)
            {
                if (balanceCoin == 0)
                    break;

                if (DB[i].Type == "Buy" && DB[index].Date.AddMinutes(HoldTime) < DB[i].Date)
                {
                    MakePurchaseSELL(i);
                }

            }

            lastIndex = i;
            AddTH(index, "Sell");
            return lastIndex;
        }
        

        private void MakePurchaseBUY(int i)
        {
            feeUSD = FEE * DB[i].Total;
            totalFee += feeUSD;
            balanceUSD -= feeUSD;
            if (balanceUSD > DB[i].Total)
            {
                balanceUSD -= DB[i].Total;
                balanceCoin += DB[i].Amount;

                totalTotal += DB[i].Total;
                totalAmount += DB[i].Amount;
            }
            else
            {
                balanceCoin += (balanceUSD/ DB[i].Rate);
                balanceUSD = 0;

                totalAmount += (balanceUSD / DB[i].Rate);
            }
            ABSRate = ABSRate == 0 ? DB[i].Rate : (DB[i].Rate + ABSRate) / 2;
        }

        private void MakePurchaseSELL(int i)
        {
            feeCoin = 0.002 * DB[i].Amount;
            totalFee += feeCoin;
            balanceCoin -= feeCoin;
            if (balanceCoin > DB[i].Amount)
            {
                balanceCoin -= DB[i].Amount;
                totalAmount += DB[i].Amount;
                balanceUSD += DB[i].Total;
                totalTotal += DB[i].Total;

            }
            else
            {
                balanceUSD += (balanceCoin * DB[i].Rate);
                totalTotal += (balanceCoin * DB[i].Rate);
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
                Balance = balanceUSD
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

        public double GetBalance() => balanceUSD;


    }
}