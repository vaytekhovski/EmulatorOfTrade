using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;

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
        public ActionResult returnTradeHistory()
        {
            
            IEnumerable<TradeHistory> histories = db.Histories;
            ViewBag.histories = histories;
         
            return View();
        }

    }
}