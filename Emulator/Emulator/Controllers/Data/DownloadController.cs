using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;


namespace Emulator.Controllers.Data
{
    public class DownloadController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TradeHistoryDownload(DateTime StartDate, DateTime EndDate, string Pair)
        {
            List<TradeHistory> lst = DownloadTradeHistory.CycleDownloadData(StartDate, EndDate, Pair);

            foreach (var DBitem in OwnDataBase.database.Histories)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (DBitem.GlobalTradeId == lst[i].GlobalTradeId)
                    {
                        lst.RemoveAt(i);
                    }
                }
            }

            IEnumerable<TradeHistory> histories = OwnDataBase.database.Histories;

            OwnDataBase.database.Histories.AddRange(lst);
            OwnDataBase.database.SaveChanges();
            histories = OwnDataBase.database.Histories.SqlQuery("select * from TradeHistories Order by [Date]");
            
            OwnDataBase.database.SaveChanges();
            ViewBag.status = $"Download trade history {Pair} ended";
            return View();
        }
    }
}