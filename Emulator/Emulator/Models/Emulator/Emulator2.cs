using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Emulator.Models.Emulator
{
    public class Emulator2
    {
        public Emulator2() { }

        List<QuickType.TradeHistory> DB = OwnDataBase.database.Histories.OrderBy(history => history.Date).ToList();

        string Coin;

        DateTime StartTime, EndTime;

        double Diff;

        double CheckTime, BuyTime, HoldTime;

        double balanceUSD, balanceCoin;
        double feeUSD = 0, feeCoin = 0;


        public Emulator2(string _Coin, DateTime _StartTime, DateTime _EndTime, double _Diff, double _CheckTime, double _BuyTime, double _HoldTime, double _balance)
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
            for(int index = 0; index < DB.Count; index++)
            {
                if(IsDiff(index))
                {

                }
            }
        }

        private bool IsDiff(int index)
        {
            bool diff = false;
            double diffValue = 0;

            double cr = CurrentRate(index);
            Debug.WriteLine(cr);
            for (double checkLength = CheckTime; checkLength > 0; checkLength -= 0.5)
            {
                double ctr = CheckTimeRate(index, checkLength);
                Debug.WriteLine(ctr);
            }
            Debug.WriteLine(" ");
            Debug.WriteLine(" ");
            Debug.WriteLine(" ");

            return diff;
        }

        private double CheckTimeRate(int index, double checkLength)
        {
            double rate = 0;

            int min = (int)checkLength;
            int sec = (((checkLength) - min) * 10) == 5 ? 30 : 0;

            DateTimeOffset currentTime = DB[index].Date;
            TimeSpan checkTime = new TimeSpan(0, min, sec);
            DateTimeOffset elementTime;

            for (int i = index; i >= 0; i--)
            {
                elementTime = DB[i].Date;

                if (currentTime - elementTime >= checkTime)
                {
                    DB[i].Rate = DB[i].Rate.Replace('.', ',');
                    rate = double.Parse(DB[i].Rate);
                    break;
                }
            }
            
            return rate;
        }

        private double CurrentRate(int index)
        {
            double rate = 0;

            DB[index].Rate = DB[index].Rate.Replace('.', ',');
            rate = double.Parse(DB[index].Rate);

            return rate;
        }

    }
}