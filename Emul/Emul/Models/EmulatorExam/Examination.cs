using System;

namespace Emul.Models.EmulatorExam
{
    public class Examination
    {
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public double Diff { get; set; }
        public double CheckTime { get; set; }
        public double BuyTime { get; set; }
        public double HoldTime { get; set; }
        public double Balance { get; set; }
    }
}