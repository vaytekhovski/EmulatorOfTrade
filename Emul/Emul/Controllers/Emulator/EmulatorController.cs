using Emul.Models.Emulator;
using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            
            Models.Emulator.Emulator2 emulator = new Models.Emulator.Emulator2(Parser.DB);
            emulator.Settings(StartDate, EndDate, double.Parse(diff), double.Parse(checkTime), double.Parse(buyTime), double.Parse(holdTime), double.Parse(balance));
            emulator.MakeMoney();
            ViewBag.balance = emulator.BalanceUSD;
            return View();
        }
    }
}