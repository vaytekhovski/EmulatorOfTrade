using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace Emulator.Controllers
{
    public class EmulatorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Emulation(string Pair, DateTime StartDate, DateTime EndDate, string diff, string checkTime, string buyTime, string holdTime, string balance)
        {
            Debug.WriteLine("parse DB to LIST started");
            List<Coin_TH> Coin_DB = new List<Coin_TH>();
            Coin_DB = OwnDataBase.database.TradeHistory.OrderBy(history => history.Date).Where(hisroty => hisroty.CurrencyName == Pair).ToList();
            Debug.WriteLine("parse DB to LIST ended");


            Models.Emulator.Emulator2 emulator = new Models.Emulator.Emulator2(Coin_DB);

            Debug.WriteLine("set settings");

            emulator.Settings(StartDate, EndDate, double.Parse(diff), double.Parse(checkTime), double.Parse(buyTime), double.Parse(holdTime), double.Parse(balance));
            Debug.WriteLine("start emulation");
            
            emulator.MakeMoney();
            
            ViewBag.balance = emulator.GetBalance();
            ViewBag.TradeHistory = emulator.TradeHistory;
            
            return View();
        }
    }
}