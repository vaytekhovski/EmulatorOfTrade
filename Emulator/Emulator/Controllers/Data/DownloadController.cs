using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;
using System.Diagnostics;

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

            DateTime start, end;
            start = EndDate;
            end = EndDate;
            IEnumerable<TradeHistory> histories = OwnDataBase.database.Histories;
            List<TradeHistory> lst;
            int j = 0;
            do
            {
                end = EndDate.AddDays(-j);

                if (start.AddDays(-10).Date < StartDate)
                    start = StartDate;
                else
                    start = end.AddDays(-10);

                
                lst = DownloadTradeHistory.CycleDownloadData(start, end, Pair);

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
                
                OwnDataBase.database.Histories.AddRange(lst);
                OwnDataBase.database.SaveChanges();


                j += 10;
            } while (start != StartDate);
            
            ViewBag.status = $"Download trade history {Pair} ended";
            return View();
        }
    }
}