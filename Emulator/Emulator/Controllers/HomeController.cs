﻿using System;
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
        TradeContext db = new TradeContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult returnChartData(DateTime StartDate, DateTime EndDate, string FirstPair, string SecondPair)
        {

            Database.SetInitializer(new ChartDataDbInitializer(StartDate, EndDate,FirstPair,SecondPair));

            IEnumerable<ChartData> charts = db.ChartDatas;
            ViewBag.charts = charts;
            return View();
        }


       
        [HttpPost]
        public ActionResult returnTradeHistory(DateTime StartDate, DateTime EndDate, string FirstPair, string SecondPair)
        {
            Database.SetInitializer(new TradeHistoryDbInitializer(StartDate, EndDate,FirstPair,SecondPair));
            IEnumerable<TradeHistory> histories = db.Histories;
            ViewBag.histories = histories;
         
            return View();
        }

    }
}