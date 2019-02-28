using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            OwnDataBase.database.TradeHistory.RemoveRange(OwnDataBase.database.TradeHistory.Where(o => o.CurrencyName == Pair));
            
            OwnDataBase.database.SaveChanges();

            ViewBag.status = $"Clear trade history {Pair} ended";
            return View();
        }
    }
}