using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;

namespace TradeEmulatorMVC.Controllers
{
    public class HomeController : Controller
    {
        TradeContext db = new TradeContext();

        public ActionResult Index(string SortOrder)
        {

            return View();
        }

        [HttpPost]
        public ActionResult returnChartData(DateTime StartDate, DateTime EndDate, string FirstPair, string SecondPair)
        {
            Database.SetInitializer(new ChartDataDbInitializer(StartDate, EndDate,FirstPair,SecondPair));
            IEnumerable<ChartData> charts = db.ChartDatas;
            ViewBag.charts = charts;

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.FirstPair = FirstPair;
            ViewBag.SecondPair = SecondPair;

            return View();
        }
       
        [HttpPost]
        public ActionResult returnTradeHistory(DateTime StartDate, DateTime EndDate, string FirstPair, string SecondPair)
        {
           // Database.SetInitializer(new TradeHistoryDbInitializer(StartDate, EndDate,FirstPair,SecondPair));
            
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.FirstPair = FirstPair;
            ViewBag.SecondPair = SecondPair;

            
            List<TradeHistory> lst = DownloadTradeHistory.CycleDownloadData(StartDate, EndDate, FirstPair, SecondPair);
            
            foreach (var DBitem in db.Histories)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (DBitem.GlobalTradeId == lst[i].GlobalTradeId)
                    {
                        lst.RemoveAt(i);
                    }
                }
            }

            IEnumerable<TradeHistory> histories = db.Histories;
            
            db.Histories.AddRange(lst);
            db.SaveChanges();
            histories = db.Histories.SqlQuery("select * from TradeHistories Order by [Date]");

            db.SaveChanges();
            
            return View(histories);
        }

    }
}