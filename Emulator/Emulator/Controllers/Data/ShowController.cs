using System;
using System.Collections.Generic;
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

        public ActionResult ShowTradeHistiories()
        {
            IEnumerable<BTC_TH> histories = OwnDataBase.database.BTC_TradeHistory;
            histories = OwnDataBase.database.BTC_TradeHistory.SqlQuery("select * from TradeHistories Order by [Date]");
            ViewBag.hist = histories;
            return View();
        }
    }
}