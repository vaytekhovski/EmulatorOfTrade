using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;

namespace TradeEmulatorMVC.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult returnChartData()
        {

            return View();
        }


        TradeContext db = new TradeContext();
        [HttpPost]
        public ActionResult returnTradeHistory(DateTime StartDate, DateTime EndDate)
        {
            Database.SetInitializer(new TradeHistoryDbInitializer(StartDate, EndDate));
            IEnumerable<TradeHistory> histories = db.Histories;
            ViewBag.histories = histories;

            return View();
        }

    }
}