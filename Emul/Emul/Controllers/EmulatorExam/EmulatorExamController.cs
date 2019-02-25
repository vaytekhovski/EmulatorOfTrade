using Emul.Models.Emulator;
using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
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

        public ActionResult Examination(string Pair, DateTime StartDate, DateTime EndDate, string diffFrom, string diffTo, string diffStep, string checkTimeFrom, string checkTimeTo, string checkTimeStep, string buyTimeFrom, string buyTimeTo, string buyTimeStep, string holdTimeFrom, string holdTimeTo, string holdTimeStep, string balance)
        {
//<<<<<<< HEAD

            List<BTC_TH> BTC_DB = new List<BTC_TH>();
            List<XRP_TH> XRP_DB = new List<XRP_TH>();
            List<ETH_TH> ETH_DB = new List<ETH_TH>();

            switch (Pair)
            {
                case "BTC":
                    BTC_DB = OwnDataBase.database.BTC_TradeHistory.OrderBy(history => history.Date).ToList();
                    Parser.Parse(BTC_DB);
                    break;

                case "XRP":
                    XRP_DB = OwnDataBase.database.XRP_TradeHistory.OrderBy(history => history.Date).ToList();
                    Parser.Parse(XRP_DB);
                    break;

                case "ETH":
                    ETH_DB = OwnDataBase.database.ETH_TradeHistory.OrderBy(history => history.Date).ToList();
                    Parser.Parse(ETH_DB);
                    break;

                default:
                    break;
            }

         
            Models.EmulatorExam.EmulatorExam emulatorexam = new Models.EmulatorExam.EmulatorExam(Parser.DB);
            emulatorexam.Settings(StartDate, EndDate, double.Parse(diffFrom), double.Parse(diffTo), double.Parse(diffStep), double.Parse(checkTimeFrom), double.Parse(checkTimeTo), double.Parse(checkTimeStep), double.Parse(buyTimeFrom), double.Parse(buyTimeTo), double.Parse(buyTimeStep), double.Parse(holdTimeFrom), double.Parse(holdTimeTo), double.Parse(holdTimeStep), double.Parse(balance));
            emulatorexam.StartExamination();
            ViewBag.balances = emulatorexam.balances;
//=======
//            //Models.EmulatorExam.EmulatorExam emulator = new Models.EmulatorExam.EmulatorExam();
//            //emulator.Settings(Pair, StartDate, EndDate, double.Parse(diffFrom), double.Parse(diffTo), double.Parse(diffStep), double.Parse(checkTimeFrom), double.Parse(checkTimeTo), double.Parse(checkTimeStep), double.Parse(buyTimeFrom), double.Parse(buyTimeTo), double.Parse(buyTimeStep), double.Parse(holdTimeFrom), double.Parse(holdTimeTo), double.Parse(holdTimeStep), double.Parse(balance));
//            //emulator.MakeMoney();
//>>>>>>> Andrey
            return View();
        }
    }

}