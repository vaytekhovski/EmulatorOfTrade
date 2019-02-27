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
     
            switch (Pair)
            {
                case "BTC":
                    {
                       histories = OwnDataBase.database.TradeHistory.SqlQuery("select * from Coin_TH where CurrencyName = 'BTC' order by [Date] ");
                        ViewBag.hist = histories;
                    }
                    break;
                case "XRP":
                    {
                        histories = OwnDataBase.database.TradeHistory.SqlQuery("select * from Coin_TH where CurrencyName = 'XRP' order by [Date] ");
                        ViewBag.hist = histories;
                    }
                    break;
                case "ETH":
                    {
                        histories = OwnDataBase.database.TradeHistory.SqlQuery("select * from Coin_TH where CurrencyName = 'ETH' order by [Date] ");
                        ViewBag.hist = histories;
                    }
                    break;
                default:
                    break;
            }
            return View();
        }
    }
}