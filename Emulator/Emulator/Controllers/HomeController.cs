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
            IEnumerable<ChartData> charts = db.ChartDatas;
            ViewBag.charts = charts;
            return View();
        }


        TradeContext db = new TradeContext();
        [HttpPost]
        public ActionResult returnTradeHistory(DateTime StartDate, DateTime EndDate)
        {
<<<<<<< HEAD
            
=======
            Database.SetInitializer(new TradeHistoryDbInitializer(StartDate, EndDate));
>>>>>>> fddbc687aa235351b43b0af5ba7ed0577da3b641
            IEnumerable<TradeHistory> histories = db.Histories;
            ViewBag.histories = histories;
         
            return View();
        }

    }
}