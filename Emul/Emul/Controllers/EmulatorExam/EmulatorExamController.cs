using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ActionResult Examination(string Pair, DateTime StartDate, DateTime EndDate, bool SaveData, string diffFrom, string diffTo, string diffStep, string diffCheckFrom, string diffCheckTo, string diffCheckStep, string checkTimeFrom, string checkTimeTo, string checkTimeStep, string buyTimeFrom, string buyTimeTo, string buyTimeStep, string holdTimeFrom, string holdTimeTo, string holdTimeStep, string balance)
        {
            Debug.WriteLine("parse DB to LIST started");
            var Coin_DB = new List<Coin_TH>();
            Coin_DB = OwnDataBase.database.TradeHistory.OrderBy(history => history.Date).Where(hisroty => hisroty.CurrencyName == Pair).ToList();

            Debug.WriteLine("parse DB to LIST ended");

            var emulatorexam = new Models.EmulatorExam.EmulatorExam(Coin_DB);

            Debug.WriteLine("set settings");
            emulatorexam.Settings(StartDate, EndDate, SaveData, double.Parse(diffFrom), double.Parse(diffTo), double.Parse(diffStep), double.Parse(diffCheckFrom), double.Parse(diffCheckTo), double.Parse(diffCheckStep), double.Parse(checkTimeFrom), double.Parse(checkTimeTo), double.Parse(checkTimeStep), double.Parse(buyTimeFrom), double.Parse(buyTimeTo), double.Parse(buyTimeStep), double.Parse(holdTimeFrom), double.Parse(holdTimeTo), double.Parse(holdTimeStep), double.Parse(balance));
            Debug.WriteLine("start examination");
            emulatorexam.StartExamination();

            ViewBag.examinations = OwnDataBase.database.Examinations.OrderByDescending(value => value.Balance);
            return View();
          
        }

        public ActionResult ShowResults()
        {
            ViewBag.examinations = OwnDataBase.database.Examinations.OrderByDescending(value => value.Balance);
            return View();
        }

        public ActionResult ClearResults()
        {
            OwnDataBase.database.Examinations.DeleteFromQuery();
            OwnDataBase.database.TradeHistories.DeleteFromQuery();
            //OwnDataBase.database.SaveChanges();
            OwnDataBase.database.BulkSaveChanges();


            ViewBag.status = $"Deleting examination history canceled";
            return View();
        }
    }

}