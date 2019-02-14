using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;

namespace Emulator.Controllers.Data
{
    public class ClearController : Controller
    {
        // GET: Clear
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClearTradeHistory()
        {
            IEnumerable<TradeHistory> histories = OwnDataBase.database.Histories;

            histories = OwnDataBase.database.Histories.SqlQuery("truncate table TradeHistories");

            OwnDataBase.database.SaveChanges();
            ViewBag.status = $"DataBase was cleared";
            return View();
        }
    }
}