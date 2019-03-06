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
    public class EmulatorExam
    {

        public readonly List<Examination> examinations = new List<Examination>();
        private  List<Coin_TH> DB = new List<Coin_TH>();

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
        private  double buyTimeStep;

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

        public EmulatorExam(List<Coin_TH> _DB)
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

            countDiff = (int)((diffTo + 0.1 - diffFrom) / diffStep);
            countCheck = (int)((checkTimeTo + 1 - checkTimeFrom) / checkTimeStep);
            countBuy = (int)((buyTimeTo + 1 - buyTimeFrom) / buyTimeStep);
            countHold = (int)((holdTimeTo + 1 - holdTimeFrom) / holdTimeStep);
            countCycles = countDiff * countCheck * countBuy * countHold;


        }

        private int countOfThreads = 20;
        private int index;

        public void StartExamination()
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

                            StartEmulation(emulator, indexDiff, indexCheck, indexBuy, indexHold);


                            SW.Stop();
                            OwnDataBase.database.Examinations.Add(NewElement(indexDiff, indexCheck, indexBuy, indexHold, emulator.GetBalance()));
                            if (SaveData)
                            {
                                Debug.WriteLine("save th");

                                //OwnDataBase.database.TradeHistories.AddRange(emulator.TradeHistory);
                                OwnDataBase.database.BulkInsert(emulator.TradeHistory);
                            }
                            OwnDataBase.database.BulkSaveChangesAsync();

                            index++;
                            Debug.WriteLine(SW.ElapsedMilliseconds);
                        }
                    }
                }
            }
            DB.Clear();
        }
        

        private void StartEmulation(Emulator2 emulator, double indexDiff, double indexCheck, double indexBuy, double indexHold)
        {
            emulator.Settings(StartDate, EndDate, SaveData,  index, indexDiff, indexCheck, indexBuy, indexHold, balance);
            emulator.MakeMoney();


            Debug.WriteLine($"[{index}/{countCycles}] Diff: {indexDiff}, CheckTime: {indexCheck}, Buytime: {indexBuy}, HoldTime: {indexHold}, Balance: {emulator.GetBalance()}");
        }

        private Examination NewElement(double indexDiff, double indexCheck, double indexBuy, double indexHold, double balance)
        {
            return new Examination
            {
                EmulationNumber = index,
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