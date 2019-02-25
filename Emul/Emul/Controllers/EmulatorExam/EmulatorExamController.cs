using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emul.Controllers.EmulatorExam
{
 
        public class EmulatorExamController : Controller
        {
            public ActionResult Index()
            {
                return View();
            }

            public ActionResult Main()
            {
                return View();
            }

        public ActionResult Examination(string Pair, DateTime StartDate, DateTime EndDate, string diff, string checkTime, string buyTime, string holdTime, string balance)
        {
            Models.EmulatorExam.EmulatorExam emulator = new Models.EmulatorExam.EmulatorExam();

            Models.Emulator.Emulator2 emulator = new Models.Emulator.Emulator2();
            emulator.Settings(Pair, StartDate, EndDate, double.Parse(diff), double.Parse(checkTime), double.Parse(buyTime), double.Parse(holdTime), double.Parse(balance));
            emulator.MakeMoney();
            return View();
        }
    }

}