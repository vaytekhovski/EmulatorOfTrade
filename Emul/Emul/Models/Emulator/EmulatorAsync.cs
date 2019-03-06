using Emul.Models.DataBase.DBModels;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Emulator.Models.Emulator
{
    public class EmulatorAsync
    {
        public static List<TH> TradeHistory = new List<TH>();

        private static List<Coin_TH> DB = new List<Coin_TH>();

        private static int Id;

        private static DateTime StartTime;
        private static DateTime EndTime;

        public static double Diff;
        private static double PersentDiff;

        public static double CheckTime;
        public static double BuyTime;
        public static double HoldTime;
        private static double balanceCoin;
        private static double balanceUSD;

        private static double feeUSD;
        private static double feeCoin;
        private const double FEE = 0.002;

        private static int startIndex;
        private static int lastIndex;

        private static double totalAmount;
        private static double totalTotal;
        private static double totalFee;
        private static double ABSRate;

        private static bool SaveData;

        private static bool isDiff = false;

        private static int index = 0;


        private static DateTimeOffset previusDate;


        public EmulatorAsync() { }

        public EmulatorAsync(List<Coin_TH> _DB)
        {
            DB = _DB ?? throw new ArgumentNullException(nameof(_DB));
        }

        public static void Settings(DateTime _StartTime, DateTime _EndTime, bool _SaveData, int _Id, double _Diff, double _CheckTime, double _BuyTime, double _HoldTime, double _balance)
        {
            StartTime = _StartTime;
            EndTime = _EndTime;

            SaveData = _SaveData;

            Id = _Id;

            Diff = _Diff;
            PersentDiff = Diff / 100;

            CheckTime = _CheckTime;
            BuyTime = _BuyTime;
            HoldTime = _HoldTime;
            balanceUSD = _balance;

            previusDate = _StartTime;

            GenerateIndexes();

        }

        private static async void GenerateIndexes()
        {
            await Task.Run(() => GenerateStartIndex());
            await Task.Run(() => GenerateLastIndex());
            
        }


        public void MakeMoney()
        {
            for (index = startIndex; index < lastIndex; index++)
            {
                if (previusDate.AddMinutes(1) <= DB[index].Date)
                {
                    IsDiff();
                    if (isDiff && DB[index].Type == "Sell")
                    {
                        Buy();
                        Sell();

                    }
                }
            }
        }

        static double checkTimeRate = 0;

        private static async void IsDiff()
        {
            double currentRate = CurrentRate();
            for (var checkLength = 0.5; checkLength < CheckTime; checkLength += 0.5)
            {
                await Task.Run(() => CheckTimeRate(checkLength));
                if (checkTimeRate < currentRate - (currentRate * PersentDiff) && currentRate != 0 && checkTimeRate != 0)
                {
                    isDiff = true;
                    break;
                }
            }

            previusDate = DB[index].Date;
            
        }

        private static void CheckTimeRate(double checkLength)
        {
            // Дата элемента, rate которого мы хотим получить
            DateTimeOffset checkDate = DB[index].Date.AddMinutes(-checkLength);

            // Двигаемся от текущего элемента
            for (int i = index; i > 0; i--)
            {
                // Проверяем, дата i-го элемента меньше либо равна даты интересующего нас элемента
                if (DB[i].Rate != 0 && DB[i].Date <= checkDate)
                {
                    // Если да, то записываем rate и выходим
                    checkTimeRate = DB[i].Rate;
                    break;
                }
            }

            // Алгорит работает медленно, в связи в необходимостью проверки всех элементов
            // в диапазоне от DB[index].Date до DB[index].Date - checkLength

        }

        private static double CurrentRate() => DB[index].Rate;

        private static async void Buy()
        {
            int i;
            for (i = index; i < DB.Count; i++)
            {
                if (balanceUSD == 0)
                    break;

                if (DB[i].Type == "Sell")
                {
                    if (DB[index].Date.AddMinutes(BuyTime) > DB[i].Date)
                    {
                        await Task.Run(() => MakePurchaseBUY(i));
                    }
                    else
                        break;
                }


            }

            if (SaveData)
                AddTH(i, "Buy");
        }

        private static async void Sell()
        {
            int i = 0;
            for (i = index; i < DB.Count; i++)
            {
                if (balanceCoin == 0)
                    break;

                if (DB[i].Type == "Buy" && DB[index].Date.AddMinutes(HoldTime) < DB[i].Date)
                {
                    await Task.Run(() => MakePurchaseSELL(i));
                }

            }

            if (SaveData)
                AddTH(i, "Sell");
        }


        private static void MakePurchaseBUY(int i)
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
                balanceCoin += (balanceUSD / DB[i].Rate);
                balanceUSD = 0;

                totalAmount += (balanceUSD / DB[i].Rate);
            }
            ABSRate = ABSRate == 0 ? DB[i].Rate : (DB[i].Rate + ABSRate) / 2;
        }

        private static void MakePurchaseSELL(int i)
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

        private static void AddTH(int index, string Type)
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

        private static void BreakValues()
        {
            totalAmount = 0;
            totalFee = 0;
            totalTotal = 0;
            ABSRate = 0;
        }

        private static void GenerateStartIndex()
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

        private static void GenerateLastIndex()
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