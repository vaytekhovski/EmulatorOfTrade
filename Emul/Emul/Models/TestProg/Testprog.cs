using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Emul.Models.TestProg
{
    public class TestProg
    {

        public TestProg() { }

        List<BTC_TH> DB = OwnDataBase.database.BTC_TradeHistory.OrderBy(history => history.Date).ToList();

        string Coin;

        DateTime StartTime, EndTime;

        double DiffFrom, DiffTo, DiffStep;

        double CheckTimeFrom, CheckTimeTo, CheckTimeStep, BuyTimeFrom, BuyTimeTo, BuyTimeStep, HoldTimeFrom, HoldTimeTo, HoldTimeStep;

        double balanceUSD, balanceCoin;
        double feeUSD = 0, feeCoin = 0;


        public TestProg(double _DiffFrom, double _DiffTo, double _DiffStep, string _Coin, DateTime _StartTime, DateTime _EndTime, double _Diff, double _CheckTimeFrom, double _CheckTimeTo, double _CheckTimeStep, double _BuyTimeFrom, double _BuyTimeTo, double _BuyTimeStep, double _HoldTimeFrom, double _HoldTimeTo, double _HoldTimeStep, double _balance)
        {
            Coin = _Coin;
            StartTime = _StartTime;
            EndTime = _EndTime;
            DiffFrom = _DiffFrom;
            DiffTo = _DiffTo;
            DiffStep = _DiffStep;
            CheckTimeFrom = _CheckTimeFrom;
            CheckTimeTo = _CheckTimeTo;
            CheckTimeStep = _CheckTimeStep;
            BuyTimeFrom = _BuyTimeFrom;
            BuyTimeTo = _BuyTimeTo;
            BuyTimeStep = _BuyTimeStep;
            HoldTimeFrom = _HoldTimeFrom;
            HoldTimeTo = _HoldTimeTo;
            HoldTimeStep = _HoldTimeStep;
            balanceUSD = _balance;

            StartTime = new DateTime(_StartTime.Year, _StartTime.Month, _StartTime.Day, 0, 01, 00);

        }


        public void MakeMoney()
        {
            for (int index = 0; index < DB.Count; index++)
            {
                if (IsDiff(index))
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