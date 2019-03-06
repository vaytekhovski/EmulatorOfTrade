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

        private int Id;

        private DateTime StartTime;
        private DateTime EndTime;

        public double Diff;
        private double PersentDiff;

        public double CheckTime;
        public double BuyTime;
        public double HoldTime;
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

        private bool SaveData;
        private double checkDiff;


        private DateTimeOffset previusDate;
        

        public Emulator2() { }

        public Emulator2(List<Coin_TH> _DB)
        {
            DB = _DB ?? throw new ArgumentNullException(nameof(_DB));
        }

        public void Settings(DateTime _StartTime, DateTime _EndTime, double _checkDiff, bool _SaveData, int _Id, double _Diff, double _CheckTime, double _BuyTime, double _HoldTime, double _balance)
        {
            StartTime = _StartTime;
            EndTime = _EndTime;

            checkDiff = _checkDiff;

            SaveData = _SaveData;

            Id = _Id;

            Diff = _Diff;
            PersentDiff = Diff / 100;

            CheckTime = _CheckTime;
            BuyTime = _BuyTime;
            HoldTime = _HoldTime;
            balanceUSD = _balance;

            previusDate = _StartTime;

            GenerateStartIndex();
            GenerateLastIndex();
        }


        public void MakeMoney()
        {
            for (var index = startIndex; index < lastIndex; index++)
            {
                if(previusDate.AddSeconds(checkDiff) <= DB[index].Date)
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

            previusDate = DB[index].Date;

            return diff;
        }

        private double CheckTimeRate(int index, double checkLength)
        {
            double rate = 0;

            // Дата элемента, rate которого мы хотим получить
            DateTimeOffset checkDate = DB[index].Date.AddMinutes(-checkLength);
            
            // Двигаемся от текущего элемента
            for (int i = index; i > 0; i--)
            {
                // Проверяем, дата i-го элемента меньше либо равна даты интересующего нас элемента
                if (DB[i].Rate != 0 && DB[i].Date <= checkDate)
                {
                    // Если да, то записываем rate и выходим
                    rate = DB[i].Rate;
                    break;
                }
            }

            // Алгорит работает медленно, в связи в необходимостью проверки всех элементов
            // в диапазоне от DB[index].Date до DB[index].Date - checkLength
            
            return rate;
        }

        private double CurrentRate(int index) => DB[index].Rate;

        private int Buy(int index)
        {
            int whenBuy = 0;
            int i = 0;
            for (i = index; i < DB.Count; i++)
            {
                if (balanceUSD == 0)
                    break;

                if (DB[i].Type == "Sell")
                {
                    if (DB[index].Date.AddMinutes(BuyTime) > DB[i].Date)
                    {
                        MakePurchaseBUY(i);
                        whenBuy = i;
                    }
                    else
                        break;
                }
                

            }
            
            if(SaveData)
                AddTH(whenBuy, "Buy");
            return i;
        }

        private int Sell(int index)
        {
            
            int i = 0;
            int whenSell = 0;
            for (i = index; i < DB.Count; i++)
            {
                if (balanceCoin == 0)
                    break;

                if (DB[i].Type == "Buy" && DB[index].Date.AddMinutes(HoldTime) < DB[i].Date)
                {
                    MakePurchaseSELL(i);
                    whenSell = i;
                }

            }
            
            if(SaveData)
                AddTH(whenSell, "Sell");
            return i;
        }
        

        private void MakePurchaseBUY(int i)
        {
            BreakValues();

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
            BreakValues();

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
                EmulationNumber = Id,
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