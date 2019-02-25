using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Emulator.Models.Emulator
{
    public class Emulator2
    {
        public Emulator2() { }

        List<Coin_TH> DB = new List<Coin_TH>();
        
        DateTime StartTime, EndTime;

        double Diff, PersentDiff;

        double CheckTime, BuyTime, HoldTime;

        double balanceUSD, balanceCoin;
        double feeUSD = 0, feeCoin = 0;

        public double BalanceUSD { get => balanceUSD;}

        private int startIndex;
        private int lastIndex;

        public Emulator2(List<Coin_TH> _DB)
        {
            DB = _DB;
        }

        public void Settings(DateTime _StartTime, DateTime _EndTime, double _Diff, double _CheckTime, double _BuyTime, double _HoldTime, double _balance)
        {
            StartTime = _StartTime;
            EndTime = _EndTime;
            Diff = _Diff;
            PersentDiff = Diff / 100;
            CheckTime = _CheckTime;
            BuyTime = _BuyTime;
            HoldTime = _HoldTime;
            balanceUSD = _balance;

            generateStartIndex();
            generateLastIndex();
        }

        private void generateStartIndex()
        {
            for (int i = 0; i < DB.Count; i++)
            {
                if (DB[i].Date > StartTime)
                {
                    startIndex = i;
                    break;
                }
            }
        }

        private void generateLastIndex()
        {
            for(int i = startIndex; i < DB.Count; i++)
            {
                if(DB[i].Date > EndTime)
                {
                    lastIndex = i;
                    break;
                }
            }
        }
        

        public void MakeMoney()
        {
            //Debug.WriteLine(DateTime.Now + " " + balanceUSD);
            for (int index = startIndex; index < lastIndex; index++)
            {
                if(IsDiff(index) && DB[index].Type == "Sell")
                {
                    index = Buy(index);
                    index = Sell(index);
                }
            }
            Debug.WriteLine(balanceUSD);
        }
        

        private int Buy(int index)
        {
            int lastIndex = 0;
            DateTimeOffset currentTime = DB[index].Date;
            for (int i = index; i < DB.Count; i++)
            {
                if(currentTime.AddMinutes((int)BuyTime) < DB[i].Date)
                {
                    break;
                }
                else if (DB[i].Type == "Sell")
                {
                    feeUSD = 0.002 * DB[i].Total;
                    if (balanceUSD - feeUSD > DB[i].Total)
                    {
                        balanceUSD -= feeUSD;

                        balanceUSD -= DB[i].Total;
                        balanceCoin += DB[i].Amount;
                    }
                    else
                    {
                        balanceCoin += ((balanceUSD - feeUSD) / DB[i].Rate);
                        balanceUSD = 0;
                    }
                }

                if (balanceUSD == 0)
                    break;
                lastIndex = i;
            }
            
            return lastIndex;
        }

        private int Sell(int index)
        {
            int lastIndex = 0;
            DateTimeOffset currentTime = DB[index].Date;
            for (int i = index; i < DB.Count; i++)
            {
                if (currentTime.AddMinutes((int)HoldTime) < DB[i].Date)
                {
                    if (DB[i].Type == "Buy")
                    {
                        feeCoin = 0.002 * DB[i].Amount;
                        if (balanceCoin > DB[i].Amount)
                        {
                            balanceCoin -= feeCoin;

                            balanceCoin -= DB[i].Amount;
                            balanceUSD += DB[i].Total;

                        }
                        else
                        {
                            balanceUSD += ((balanceCoin - feeCoin) * DB[i].Rate);
                            balanceCoin = 0;
                        }
                    }

                    if (balanceCoin == 0)
                        break;
                }
                lastIndex = i;
            }
            //Debug.WriteLine(DB[index].Date + " SELL " + balanceUSD);
            return lastIndex;
        }

        private bool IsDiff(int index)
        {

            bool diff = false;

            double cr = CurrentRate(index);
            for (double checkLength = 0.5; checkLength < CheckTime; checkLength += 0.5)
            {
                double ctr = CheckTimeRate(index, checkLength);
                if (ctr < cr - (cr * PersentDiff) &&  cr != 0 && ctr != 0)
                {
                    diff = true;
                    //Debug.WriteLine(ctr);
                    break;
                }
            }

            return diff;
        }

        private double CheckTimeRate(int index, double checkLength)
        {
            double rate = 0;

            int min = (int)checkLength;
            int sec = (((checkLength) - min) * 10) == 5 ? 30 : 0;
            
            DateTimeOffset checkTime = DB[index].Date.AddMinutes(-min).AddSeconds(-sec);

            for(int i = index; i > 0; i--)
            {
                if(DB[i].Date < checkTime && DB[i].Rate != 0)
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