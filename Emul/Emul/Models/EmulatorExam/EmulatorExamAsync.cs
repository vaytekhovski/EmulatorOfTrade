using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using Emulator.Models.Emulator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;



namespace Emul.Models.EmulatorExam
{
    public class EmulatorExamAsync
    {

        public static readonly List<Examination> examinations = new List<Examination>();
        private static  List<Coin_TH> DB = new List<Coin_TH>();

        private static DateTime StartDate;
        private static DateTime EndDate;

        private static double diffFrom;
        private static double diffTo;
        private static double diffStep;

        private static double checkTimeFrom;
        private static double checkTimeTo;
        private static double checkTimeStep;

        private static double buyTimeFrom;
        private static double buyTimeTo;
        private static double buyTimeStep;

        private static double holdTimeFrom;
        private static double holdTimeTo;
        private static double holdTimeStep;

        private static double balance;

        private static int countDiff;
        private static int countCheck;
        private static int countBuy;
        private static int countHold;
        private static int countCycles;

        static EmulatorExamAsync()
        {

        }
        

        public void Settings(List<Coin_TH> _DB, DateTime _StartDate, DateTime _EndDate, double _DiffFrom, double _DiffTo, double _DiffStep, double _CheckTimeFrom, double _CheckTimeTo, double _CheckTimeStep, double _BuyTimeFrom, double _BuyTimeTo, double _BuyTimeStep, double _HoldTimeFrom, double _HoldTimeTo, double _HoldTimeStep, double _balance)
        {
            DB = _DB;
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

            countDiff = (int)((diffTo + 0.1 - diffFrom) / diffStep);
            countCheck = (int)((checkTimeTo + 1 - checkTimeFrom) / checkTimeStep);
            countBuy = (int)((buyTimeTo + 1 - buyTimeFrom) / buyTimeStep);
            countHold = (int)((holdTimeTo + 1 - holdTimeFrom) / holdTimeStep);
            countCycles = countDiff * countCheck * countBuy * countHold;
            
            
        }

        private static int countOfThreads = 20;
        private static int index;

        public async Task StartExaminationAsync()
        {
            index = 1;
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

                            await StartEmulation(emulator, indexDiff, indexCheck, indexBuy, indexHold);

                            SW.Stop();
                            //Debug.WriteLine($"Время, затраченное на {countOfThreads} потоков {SW.ElapsedMilliseconds} миллисекунд");
                            //Debug.WriteLine($"Время, затраченное на 1 поток {SW.ElapsedMilliseconds / countOfThreads} миллисекунд");
                            index++;


                        }
                    }
                }
            }
            OwnDataBase.database.Examinations.AddRange(examinations);
            OwnDataBase.database.SaveChanges();
            DB.Clear();
        }
        

        private static async Task StartEmulation(Emulator2 emulator, double indexDiff, double indexCheck, double indexBuy, double indexHold)
        {
            Debug.WriteLine("[" + index + "/" + countCycles + "] async STARTED");

            emulator.Settings(StartDate, EndDate, indexDiff, indexCheck, indexBuy, indexHold, balance);
            await Task.Factory.StartNew(() => emulator.MakeMoney());
            //await Task.Run(() => emulator.MakeMoney());
            examinations.Add(NewElement(indexDiff, indexCheck, indexBuy, indexHold, emulator.GetBalance()));


        }

        private static Examination NewElement(double indexDiff, double indexCheck, double indexBuy, double indexHold, double balance)
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