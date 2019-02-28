using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using Emulator.Models.DataBase.DBModels;
using System.Linq;
using System.Diagnostics;

namespace Emulator.Controllers.Data
{
    public class DownloadController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }


        private List<Coin_TH> th_list = new List<Coin_TH>();
    
        public ActionResult TradeHistoryDownload(DateTime StartDate, DateTime EndDate, string Pair)
        {
            DateTime start, end;
            start = EndDate;
            end = EndDate;

            List<Coin_TH> DB = OwnDataBase.database.TradeHistory.OrderBy(history => history.Date).ToList();
          
            int j = 0;
            do
            {
                Debug.WriteLine($"{DateTime.Now} Download trade history {start.Date} : {end.Date} started");
                
                end = EndDate.AddDays(-j);
                
                start = start.AddDays(-10).Date < StartDate ? StartDate : end.AddDays(-10);

                ConvertToTH(DownloadTradeHistory.CycleDownloadData(start, end, Pair), Pair);
                        
                for (int i = 0; i < DB.Count; i++)
                    for (int z = 0; z < th_list.Count; z++)
                        if (DB[i].GlobalTradeId == th_list[z].GlobalTradeId)
                            th_list.RemoveAt(z);

                OwnDataBase.database.TradeHistory.AddRange(th_list);
                OwnDataBase.database.SaveChanges();

                Debug.WriteLine($"{DateTime.Now} Download trade history {start.Date} : {end.Date} ended\n");

                j += 10;
            } while (start != StartDate);
            
            ViewBag.status = $"Download trade history {Pair} ended";
            return View();
        }


        void ConvertToTH(List<TradeHistory> histories, string type)
        {
           th_list.Clear();
           foreach(var item in histories)
           {
             item.Rate = item.Rate.Replace('.', ',');
             item.Amount = item.Amount.Replace('.', ',');
             item.Total = item.Total.Replace('.', ',');
                th_list.Add(new Coin_TH
                {
                  CurrencyName = type,
                  GlobalTradeId = item.GlobalTradeId,
                  TradeId = item.TradeId,
                  Date = item.Date,
                  Type = item.Type.ToString(),
                  Rate = double.Parse(item.Rate),
                  Amount = double.Parse(item.Amount),
                  Total = double.Parse(item.Total)
                });
            }
          }
        }
    }
