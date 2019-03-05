using Emulator.Models.DataBase.DBModels;
using System.Collections.Generic;
using Emulator.Models.Emulator;
using System;
using System.Diagnostics;
using Emulator.Models;
using System.Threading;

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

        private int countOfThreads = 20;

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
                            var SW = new Stopwatch();
                            SW.Start();

                            settings = new Settings(StartDate, EndDate, balance, index, countCycles);
                            settings.emulator = emulator;
                            settings.indexDiff = indexDiff;
                            settings.indexCheck = indexCheck;
                            settings.indexBuy = indexBuy;
                            settings.indexHold = indexHold;


                            Thread thread = new Thread(new ParameterizedThreadStart(settings.StartEmulation));
                            thread.Start();
                            if(index % countOfThreads == 0)
                                thread.Join();
                            
                            if(!thread.IsAlive)
                            {

                                SW.Stop();
                                Debug.WriteLine($"Время, затраченное на {countOfThreads} потоков {SW.ElapsedMilliseconds} миллисекунд");
                                Debug.WriteLine($"Время, затраченное на 1 поток {SW.ElapsedMilliseconds / countOfThreads} миллисекунд");
                            }


                            //StartEmulation(emulator, indexDiff, indexCheck, indexBuy, indexHold);
                            index++;


                        }
                    }
                }
            }
            //OwnDataBase.database.Examinations.AddRange(settings.examinations);
            OwnDataBase.database.SaveChanges();
            DB.Clear();
        }

        

    }


    public class Settings
    {
        public Settings(DateTime start, DateTime end, double _balance, int _index, int _count)
        {
            StartDate = start;
            EndDate = end;
            balance = _balance;
            index = _index;
            countCycles = _count;
        }


        public List<Examination> examinations = new List<Examination>();


        private DateTime StartDate;
        private DateTime EndDate;

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
            Debug.WriteLine("[" + index + "/" + countCycles + "] Thread STARTED");

            var sw = new Stopwatch();
            sw.Start();
            emulator.Settings(StartDate, EndDate, indexDiff, indexCheck, indexBuy, indexHold, balance);
            emulator.MakeMoney();
            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds);

            examinations.Add(NewElement(indexDiff, indexCheck, indexBuy, indexHold, emulator.GetBalance()));


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