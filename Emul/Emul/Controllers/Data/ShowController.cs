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
            IEnumerable<BTC_TH> btc_histories = OwnDataBase.database.BTC_TradeHistory;
            IEnumerable<XRP_TH> xrp_histories = OwnDataBase.database.XRP_TradeHistory;
            IEnumerable<ETH_TH> eth_histories = OwnDataBase.database.ETH_TradeHistory;
            switch (Pair)
            {
                case "BTC":
                    {
                        btc_histories = OwnDataBase.database.BTC_TradeHistory.SqlQuery("select * from BTC_TH Order by [Date]");
                        ViewBag.hist = btc_histories;
                    }
                    break;
                case "XRP":
                    {
                        xrp_histories = OwnDataBase.database.XRP_TradeHistory.SqlQuery("select * from XRP_TH Order by [Date]");
                        ViewBag.hist = xrp_histories;
                    }
                    break;
                case "ETH":
                    {
                        eth_histories = OwnDataBase.database.ETH_TradeHistory.SqlQuery("select * from ETH_TH Order by [Date]");
                        ViewBag.hist = eth_histories;
                    }
                    break;
                default:
                    break;
            }
            return View();
        }
    }
}