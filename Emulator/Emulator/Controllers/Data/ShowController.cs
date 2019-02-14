﻿using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;

namespace Emulator.Controllers.Data
{
    public class ShowController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowTradeHistiories()
        {
            IEnumerable<TradeHistory> histories = OwnDataBase.database.Histories;
            histories = OwnDataBase.database.Histories.SqlQuery("select * from TradeHistories Order by [Date]");
            OwnDataBase.database.SaveChanges();
            return View(histories);
        }
    }
}