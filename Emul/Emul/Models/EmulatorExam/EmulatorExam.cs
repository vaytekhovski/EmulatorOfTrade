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
        private List<Examination> examinations = new List<Examination>();

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

        private int countDiff;
        private int countCheck;
        private int countBuy;
        private int countHold;
        private int countCycles;

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

            countDiff = (int)((diffTo + 0.1 - diffFrom)/diffStep);
            countCheck = (int)((checkTimeTo + 1 - checkTimeFrom) / checkTimeStep);
            countBuy = (int)((buyTimeTo + 1 - buyTimeFrom ) / buyTimeStep);
            countHold = (int)((holdTimeTo + 1 - holdTimeFrom) / holdTimeStep);
            countCycles = countDiff * countCheck * countBuy * countHold;
        }
        

        public void StartExamination()
        {
            int index = 1;
            var emulator = new Emulator2(DB);
            
            for (double indexDiff = diffFrom; indexDiff < diffTo + 0.1; indexDiff += diffStep)
            {
                for (double indexCheck = checkTimeFrom; indexCheck < checkTimeTo + 1; indexCheck += checkTimeStep)
                {
                    for (double indexBuy = buyTimeFrom; indexBuy < buyTimeTo + 1; indexBuy += buyTimeStep)
                    {
                        for (double indexHold = holdTimeFrom; indexHold < holdTimeTo + 1; indexHold += holdTimeStep)
                        {
                            var SW = new Stopwatch();
                            SW.Start();
                            emulator.Settings(StartDate, EndDate, indexDiff, indexCheck, indexBuy, indexHold, balance);
                            emulator.MakeMoney();

                            examinations.Add(NewElement(indexDiff, indexCheck, indexBuy, indexHold, emulator.BalanceUSD));
                            

                            SW.Stop();
                            Debug.WriteLine("[" + index + "/" + countCycles + "] " +  SW.Elapsed.Seconds + " " + emulator.BalanceUSD);
                            index++;
                        }
                    }
                }
            }
            OwnDataBase.database.Examinations.AddRange(examinations);
            OwnDataBase.database.SaveChanges();
            DB.Clear();
        }

        private Examination NewElement(double indexDiff, double indexCheck, double indexBuy, double indexHold, double balance)
        {
            return new Examination
            {
                StartDate = StartDate,
                EndDate = EndDate,
                Diff = indexDiff,
                CheckTime = indexCheck,
                BuyTime = indexBuy,
                HoldTime = indexHold,
                Balance = balance
            };
        }
    }
}