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

        private double checkDiffFrom;
        private double checkDiffTo;
        private double checkDiffStep;

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
        private double outBalance;

        private int countDiff;
        private int countCheckDiff;
        private int countCheck;
        private int countBuy;
        private int countHold;
        private int countCycles;

        private bool SaveData;

        public EmulatorExam(List<Coin_TH> _DB)
        {
            DB = _DB;
        }


        public void Settings(DateTime _StartDate, DateTime _EndDate, bool _SaveData, double _DiffFrom, double _DiffTo, double _DiffStep, double _checkDiffFrom, double _checkDiffTo, double _checkDiffStep, double _CheckTimeFrom, double _CheckTimeTo, double _CheckTimeStep, double _BuyTimeFrom, double _BuyTimeTo, double _BuyTimeStep, double _HoldTimeFrom, double _HoldTimeTo, double _HoldTimeStep, double _balance)
        {
            StartDate = _StartDate;
            EndDate = _EndDate;

            SaveData = _SaveData;

            diffFrom = _DiffFrom;
            diffTo = _DiffTo;
            diffStep = _DiffStep;

            checkDiffFrom = _checkDiffFrom;
            checkDiffTo = _checkDiffTo;
            checkDiffStep = _checkDiffStep;

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
            countCheckDiff = (int)((checkDiffTo + 10 - checkDiffFrom) / checkDiffStep);
            countCheck = (int)((checkTimeTo + 1 - checkTimeFrom) / checkTimeStep);
            countBuy = (int)((buyTimeTo + 1 - buyTimeFrom) / buyTimeStep);
            countHold = (int)((holdTimeTo + 1 - holdTimeFrom) / holdTimeStep);
            countCycles = countDiff * countCheck * countBuy * countHold * countCheckDiff;


        }
        
        private int index;

        public void StartExamination()
        {
            index = 1;
            var emulator = new Emulator2(DB);
            
            for (double indexCheckDiff = checkDiffFrom; indexCheckDiff < checkDiffTo + 0.1; indexCheckDiff += checkDiffStep)
            {
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

                                emulator.Settings(StartDate, EndDate, indexCheckDiff, SaveData, index, indexDiff, indexCheck, indexBuy, indexHold, balance);
                                emulator.MakeMoney();
                                outBalance = emulator.GetBalance();

                                examinations.Add(NewElement(indexDiff, indexCheckDiff, indexCheck, indexBuy, indexHold, outBalance));
                                
                                Debug.WriteLine($"[{index}/{countCycles}] Diff: {indexDiff}, checkDiff: {indexCheckDiff}, CheckTime: {indexCheck}, Buytime: {indexBuy}, HoldTime: {indexHold}, Balance: {outBalance}");
                                if (SaveData)
                                {
                                    Debug.WriteLine("save th");
                                    
                                    OwnDataBase.database.BulkInsert(emulator.TradeHistory);
                                }

                                index++;
                                SW.Stop();
                                Debug.WriteLine(SW.ElapsedMilliseconds);
                            }
                            OwnDataBase.database.BulkInsert(examinations);
                            examinations.Clear();
                            OwnDataBase.database.BulkSaveChanges();
                            Debug.WriteLine("SAVE DATA");
                        }
                    }
                }
            }
            
            DB.Clear();
        }
        
        private Examination NewElement(double indexDiff, double indexCheckDiff, double indexCheck, double indexBuy, double indexHold, double balance)
        {
            return new Examination
            {
                EmulationNumber = index,
                StartDate = StartDate,
                EndDate = EndDate,
                Diff = indexDiff,
                CheckDiff = indexCheckDiff,
                CheckTime = indexCheck,
                BuyTime = indexBuy,
                HoldTime = indexHold,
                Balance = balance
            };
        }

    }
}