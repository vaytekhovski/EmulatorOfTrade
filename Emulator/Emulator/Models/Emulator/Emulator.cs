using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Diagnostics;

namespace Emulator.Models.Emulator
{
    public class Emulator
    {
        public Emulator() { }

        // Валютная пара
        string Coin;

        // Время начала и конца проведения исследования
        DateTime StartTime, EndTime;

        // Процент корреляции, валидный для закупа
        double Diff;

        // Время проверки валидности, время закупки, время через которое начинается продажа
        double CheckTime, BuyTime, HoldTime;

        public Emulator(string _Coin, DateTime _StartTime, DateTime _EndTime, double _Diff, double _CheckTime, double _BuyTime, double _HoldTime)
        {
            Coin = _Coin;
            StartTime = _StartTime;
            EndTime = _EndTime;
            Diff = _Diff;
            CheckTime = _CheckTime;
            BuyTime = _BuyTime;
            HoldTime = _HoldTime;

            StartTime = new DateTime(_StartTime.Year, _StartTime.Month, _StartTime.Day, 0, 01, 00);
        }

      
        public void MakeMoney()
        {
            DateTime currentTime = StartTime;
            while(currentTime < EndTime)
            {
                // цикл, необходимый для проверки курса на всем CheckTime
                // проверка курса каждые 30 сек
                for (double tempCheckTime = CheckTime; tempCheckTime > 0; tempCheckTime -= 0.5)
                {
                    // Если курс в текущий момент / курс в (текущий момент - CheckTime) валиден то
                    if (ReturnCourseValue(currentTime) / ReturnCourseValue(currentTime.AddMinutes(-tempCheckTime)) >= Diff)
                    {
                        Buy(ref currentTime, BuyTime);
                        Sell(ref currentTime, HoldTime);
                        break;
                    }
                }
                currentTime = currentTime.AddMinutes(1);
            }
        }

        private double ReturnCourseValue(DateTime curretTime)
        {
            // Имитация возвращения курса в заданное время
            Random rnd = new Random();
            return rnd.Next();
        }

        // Имитация покупки
        private void Buy(ref DateTime curretTime, double buyTime)
        {
            // покупаем пока BuyTime не приблизился к currentTime
            double tempBuyTime = BuyTime;
            while (tempBuyTime > 0)
            {
                Debug.WriteLine(curretTime + " Buy");
                curretTime = curretTime.AddMinutes(1);
                tempBuyTime--;
            }
        }

        // Имитация продажи
        private void Sell(ref DateTime curretTime, double holdTime)
        {
            // ждем HoldTime, после чего продаем 
            curretTime = curretTime.AddMinutes(holdTime);
            Debug.WriteLine(curretTime + " Sell");
        }
    }
}