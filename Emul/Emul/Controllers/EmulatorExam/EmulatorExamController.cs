using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Examination(string Pair, DateTime StartDate, DateTime EndDate, string diffFrom, string diffTo, string diffStep, string checkTimeFrom, string checkTimeTo, string checkTimeStep, string buyTimeFrom, string buyTimeTo, string buyTimeStep, string holdTimeFrom, string holdTimeTo, string holdTimeStep, string balance)
        {
            List<Coin_TH> Coin_DB = new List<Coin_TH>();
            Coin_DB = OwnDataBase.database.TradeHistory.OrderBy(history => history.Date).ToList();

            Models.EmulatorExam.EmulatorExam emulatorexam = new Models.EmulatorExam.EmulatorExam(Coin_DB);

            emulatorexam.Settings(StartDate, EndDate, double.Parse(diffFrom), double.Parse(diffTo), double.Parse(diffStep), double.Parse(checkTimeFrom), double.Parse(checkTimeTo), double.Parse(checkTimeStep), double.Parse(buyTimeFrom), double.Parse(buyTimeTo), double.Parse(buyTimeStep), double.Parse(holdTimeFrom), double.Parse(holdTimeTo), double.Parse(holdTimeStep), double.Parse(balance));
            emulatorexam.StartExamination();

            ViewBag.examinations = OwnDataBase.database.Examinations.OrderByDescending(value => value.Balance);
            return View();
          
        }

        public ActionResult ShowResults()
        {
            ViewBag.examinations = OwnDataBase.database.Examinations.OrderByDescending(value => value.Balance);
            return View();
        }
    }

}