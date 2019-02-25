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
            emulator.Settings(Pair, StartDate, EndDate, double.Parse(diffFrom), double.Parse(diffTo), double.Parse(diffStep), double.Parse(checkTimeFrom), double.Parse(checkTimeTo), double.Parse(checkTimeStep), double.Parse(buyTimeFrom), double.Parse(buyTimeTo), double.Parse(buyTimeStep), double.Parse(holdTimeFrom), double.Parse(holdTimeTo), double.Parse(holdTimeStep), double.Parse(balance));
            emulator.MakeMoney();
            return View();
        }
    }

}