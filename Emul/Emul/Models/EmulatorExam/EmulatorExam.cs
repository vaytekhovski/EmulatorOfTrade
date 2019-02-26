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
        List<Coin_TH> DB = new List<Coin_TH>();
        public List<Examination> examinations = new List<Examination>();
        

        public EmulatorExam(List<Coin_TH> _DB)
        {
            DB = _DB;
        }

        DateTime StartDate, EndDate;
        double diffFrom, diffTo, diffStep;
        double checkTimeFrom, checkTimeTo, checkTimeStep;
        double buyTimeFrom, buyTimeTo, buyTimeStep;
        double holdTimeFrom, holdTimeTo, holdTimeStep;
        double balance;

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
            Emulator2 emulator = new Emulator2(DB);

            for (double i = diffFrom; i < diffTo; i+=diffStep)
            {
                for (double j = checkTimeFrom; j < checkTimeTo; j += checkTimeStep)
                {
                    for (double k = buyTimeFrom; k < buyTimeTo; k += buyTimeStep)
                    {
                        for (double l = holdTimeFrom; l < holdTimeTo; l += holdTimeStep)
                        {
                            //Debug.WriteLine($"Starting emulation with diff: {i}, checkTime: {j}, buyTime: {k}, holdTime: {l}");
                            emulator.Settings(StartDate, EndDate, i, j, k, l, balance);
                            emulator.MakeMoney();
                            examinations.Add(new Examination {
                                StartDate = StartDate,
                                EndDate = EndDate,
                                Diff = i,
                                CheckTime = j,
                                BuyTime = k,
                                HoldTime = l,
                                Balance = emulator.BalanceUSD
                            });
                            OwnDataBase.database.Examinations.Add(examinations[examinations.Count - 1]);
                            OwnDataBase.database.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}