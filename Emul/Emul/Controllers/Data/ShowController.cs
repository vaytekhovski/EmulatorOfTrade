using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;
using Emulator.Models.DataBase.DBModels;

namespace Emulator.Controllers.Data
{
    public class ShowController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowTradeHistiories(string Pair)
        {
            IEnumerable<Coin_TH> histories = OwnDataBase.database.TradeHistory;

            histories = OwnDataBase.database.TradeHistory.SqlQuery($"select * from Coin_TH where CurrencyName = '{Pair}' order by [Date] ");
            ViewBag.hist = histories;

            return View();
        }
    }
}