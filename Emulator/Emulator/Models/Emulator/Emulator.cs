using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

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
        }

      
        public void MakeMoney()
        {
            for (DateTime currentTime = StartTime; currentTime < EndTime; currentTime.AddMinutes(1))
            {
                // цикл, необходимый для проверки курса на всем CheckTime
                // проверка курса каждые 30 сек
                for(double tempCheckTime = CheckTime; tempCheckTime >= 0; tempCheckTime -= 0.5)
                {
                    // Если курс в текущий момент / курс в (текущий момент - CheckTime) валиден то
                    if (ReturnCourseValue(currentTime) / ReturnCourseValue(currentTime.AddMinutes(-tempCheckTime)) >= Diff)
                    {
                        Buy(currentTime, BuyTime);
                        Sell(currentTime, HoldTime);
                    }
                }
            }
        }

        private double ReturnCourseValue(DateTime curretTime)
        {
            // Имитация возвращения курса в заданное время
            Random rnd = new Random();
            return rnd.Next();
        }

        // Имитация покупки
        private void Buy(DateTime curretTime, double buyTime)
        {
            // покупаем пока BuyTime не приблизился к currentTime
            Thread.Sleep((int)buyTime * 3600);
        }

        // Имитация продажи
        private void Sell(DateTime curretTime, double holdTime)
        {
            // ждем HoldTime, после чего продаем 
            Thread.Sleep((int)holdTime * 3600);
        }
    }
}