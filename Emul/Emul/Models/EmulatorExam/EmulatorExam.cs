using Emulator.Models.DataBase.DBModels;
using System.Collections.Generic;
using Emulator.Models.Emulator;
using System;
using System.Diagnostics;
using Emulator.Models;

namespace Emul.Models.EmulatorExam
{
    public class EmulatorExam
    {
        private readonly List<Coin_TH> DB = new List<Coin_TH>();

        private DateTime StartDate;
        private DateTime EndDate;

        private double diffFrom;
        private double diffTo;
        private double diffStep;

        private double checkTimeFrom;
        private double checkTimeTo;
        private double checkTimeStep;

        private double buyTimeFrom;
        private double buyTimeTo;
        private double buyTimeStep;

        private double holdTimeFrom;
        private double holdTimeTo;
        private double holdTimeStep;

        private double balance;

        public EmulatorExam(List<Coin_TH> _DB)
        {
            DB = _DB;
        }

        public void Settings(DateTime _StartDate, DateTime _EndDate, double _DiffFrom, double _DiffTo, double _DiffStep, double _CheckTimeFrom, double _CheckTimeTo, double _CheckTimeStep, double _BuyTimeFrom, double _BuyTimeTo, double _BuyTimeStep, double _HoldTimeFrom, double _HoldTimeTo, double _HoldTimeStep, double _balance)
        {
            StartDate = _StartDate;
            EndDate = _EndDate;

            diffFrom = _DiffFrom;
            diffTo = _DiffTo;
            diffStep = _DiffStep;

            checkTimeFrom = _CheckTimeFrom;
            checkTimeTo = _CheckTimeTo;
            checkTimeStep = _CheckTimeStep;

            buyTimeFrom = _BuyTimeFrom;
            buyTimeTo = _BuyTimeTo;
            buyTimeStep = _BuyTimeStep;

            holdTimeFrom = _HoldTimeFrom;
            holdTimeTo = _HoldTimeTo;
            holdTimeStep = _HoldTimeStep;

            balance = _balance;
        }
        

        public void StartExamination()
        {
            for (double i = diffFrom; i < diffTo; i += diffStep)
            {
                for (double j = checkTimeFrom; j < checkTimeTo; j += checkTimeStep)
                {
                    for (double k = buyTimeFrom; k < buyTimeTo; k += buyTimeStep)
                    {
                        for (double l = holdTimeFrom; l < holdTimeTo; l += holdTimeStep)
                        {
                            var emulator = new Emulator2(DB);

                            emulator.Settings(StartDate, EndDate, i, j, k, l, balance);
                            emulator.MakeMoney();
                            
                            OwnDataBase.database.Examinations.Add(NewElement(i, j, k, l, emulator.BalanceUSD));
                            OwnDataBase.database.SaveChanges();
                        }
                    }
                }
            }
            DB.Clear();
        }

        private Examination NewElement(double i, double j, double k, double l, double balance)
        {
            return new Examination
            {
                StartDate = StartDate,
                EndDate = EndDate,
                Diff = i,
                CheckTime = j,
                BuyTime = k,
                HoldTime = l,
                Balance = balance
            };
        }
    }
}