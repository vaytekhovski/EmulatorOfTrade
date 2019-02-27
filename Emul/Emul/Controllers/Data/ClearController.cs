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
                    OwnDataBase.database.TradeHistory.SqlQuery("delete from Coin_TH where CurrencyName =  'BTC' ");
                    break;
                case "XRP":
                    OwnDataBase.database.TradeHistory.SqlQuery("delete from Coin_TH where CurrencyName = 'XRP' ");
                    break;
                case "ETH":
                    OwnDataBase.database.TradeHistory.SqlQuery("delete from Coin_TH where CurrencyName = 'ETH' ");
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