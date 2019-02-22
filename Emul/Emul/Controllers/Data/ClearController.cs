using Emulator.Models;
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
            switch (Pair)
            {
                case "BTC":
                    OwnDataBase.database.BTC_TradeHistory.SqlQuery("delete from BTC_TH where id > 0");
                    break;
                case "XRP":
                    OwnDataBase.database.XRP_TradeHistory.SqlQuery("delete from XRP_TH where id > 0");
                    break;
                case "ETH":
                    OwnDataBase.database.ETH_TradeHistory.SqlQuery("delete from ETH_TH where id > 0");
                    break;
                default:
                    break;
            }

            OwnDataBase.database.SaveChanges();
            ViewBag.status = $"Clear trade history {Pair} ended";
            return View();
        }
    }
}