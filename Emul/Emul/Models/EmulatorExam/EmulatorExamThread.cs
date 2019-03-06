using Emulator.Models.DataBase.DBModels;
using System.Collections.Generic;
using Emulator.Models.Emulator;
using System;
using System.Diagnostics;
using Emulator.Models;
using System.Threading;
using Emul.Models.DataBase.DBModels;

namespace Emul.Models.EmulatorExam
{
    public class EmulatorExamThread
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

        private int countDiff;
        private int countCheck;
        private int countBuy;
        private int countHold;
        private int countCycles;

        private bool SaveData;

        public EmulatorExamThread(List<Coin_TH> _DB)
        {
            DB = _DB;
        }

        public void Settings(DateTime _StartDate, DateTime _EndDate, bool _SaveData, double _DiffFrom, double _DiffTo, double _DiffStep, double _CheckTimeFrom, double _CheckTimeTo, double _CheckTimeStep, double _BuyTimeFrom, double _BuyTimeTo, double _BuyTimeStep, double _HoldTimeFrom, double _HoldTimeTo, double _HoldTimeStep, double _balance)
        {
            StartDate = _StartDate;
            EndDate = _EndDate;

            SaveData = _SaveData;

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
            
            Settings settings;

            for (double indexDiff = diffFrom; indexDiff < diffTo + 0.1; indexDiff += diffStep)
            {
                for (double indexCheck = checkTimeFrom; indexCheck < checkTimeTo + 1; indexCheck += checkTimeStep)
                {
                    for (double indexBuy = buyTimeFrom; indexBuy < buyTimeTo + 1; indexBuy += buyTimeStep)
                    {
                        for (double indexHold = holdTimeFrom; indexHold < holdTimeTo + 1; indexHold += holdTimeStep)
                        {
                            var sw = new Stopwatch();
                            sw.Start();

                            settings = new Settings(StartDate, EndDate, SaveData, balance, index, countCycles);
                            settings.emulator = emulator;
                            settings.indexDiff = indexDiff;
                            settings.indexCheck = indexCheck;
                            settings.indexBuy = indexBuy;
                            settings.indexHold = indexHold;


                            Thread thread = new Thread(new ParameterizedThreadStart(settings.StartEmulation));
                            thread.Start();
                            


                            OwnDataBase.database.Examinations.Add(settings.NewElement(indexDiff, indexCheck, indexBuy, indexHold-1, emulator.GetBalance()));
                            if (SaveData)
                            {
                                Debug.WriteLine("save th");
                                //OwnDataBase.database.TradeHistories.AddRange(emulator.TradeHistory);
                                OwnDataBase.database.BulkInsert(emulator.TradeHistory);
                            }

                            OwnDataBase.database.BulkSaveChangesAsync();
                            index++;

                            sw.Stop();
                            Debug.WriteLine(sw.ElapsedMilliseconds);
                        }
                    }
                }
            }
            DB.Clear();
        }
    }


    public class Settings
    {
        public Settings(DateTime start, DateTime end, bool _SaveData, double _balance, int _index, int _count)
        {
            StartDate = start;
            EndDate = end;
            balance = _balance;
            index = _index;
            countCycles = _count;
            SaveData = _SaveData;
        }

        private DateTime StartDate;
        private DateTime EndDate;

        private bool SaveData;

        public Emulator2 emulator;
        public double indexDiff;
        public double indexCheck;
        public double indexBuy;
        public double indexHold;
        private double balance;
        private int index;
        private int countCycles;

        public void StartEmulation(object settings)
        {
            
            emulator.Settings(StartDate, EndDate, SaveData, index, indexDiff, indexCheck, indexBuy, indexHold, balance);
            emulator.MakeMoney();


            Debug.WriteLine($"[{index}/{countCycles}] Diff: {indexDiff}, CheckTime: {indexCheck}, Buytime: {indexBuy}, HoldTime: {indexHold}, Balance: {emulator.GetBalance()}");

        }


        public Examination NewElement(double indexDiff, double indexCheck, double indexBuy, double indexHold, double balance)
        {
            return new Examination
            {
                EmulationNumber = index - 1,
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