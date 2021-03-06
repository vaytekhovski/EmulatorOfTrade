﻿using Emulator.Models;
using System.Linq;
using System.Web.Mvc;

namespace Emul.Controllers.Data
{
    public class ClearController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClearTradeHistory(string Pair)
        {
            OwnDataBase.database.TradeHistory.Where(o => o.CurrencyName == Pair).DeleteFromQuery();
            OwnDataBase.database.BulkSaveChanges();

            ViewBag.status = $"Clear trade history {Pair} ended";
            return View();
        }
    }
}