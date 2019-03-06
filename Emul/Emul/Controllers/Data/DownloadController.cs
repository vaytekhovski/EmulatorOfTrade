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

        private DateTime startDate;
        private DateTime endDate;

        private double MinValue;

        private List<Coin_TH> th_list = new List<Coin_TH>();
    
        public ActionResult TradeHistoryDownload(DateTime StartDate, DateTime EndDate, string Pair, string MinValue)
        {
            this.MinValue = double.Parse(MinValue);
            startDate = EndDate;
            endDate = EndDate;

            List<Coin_TH> DB = OwnDataBase.database.TradeHistory.OrderBy(history => history.Date).ToList();
          
            int tempDays = 0;
            do
            {
                endDate = EndDate.AddDays(-tempDays);
                startDate = startDate.AddDays(-10).Date < StartDate ? StartDate : endDate.AddDays(-10);


                Debug.WriteLine($"{DateTime.Now} Download trade history {startDate.Date} : {endDate.Date} started");

                ConvertToTH(DownloadTradeHistory.CycleDownloadData(startDate, endDate, Pair), Pair);

                CheckMinValue();
                CheckExist(Pair, DB);

                //OwnDataBase.database.TradeHistory.AddRange(th_list);
                OwnDataBase.database.BulkInsert(th_list);
                OwnDataBase.database.BulkSaveChangesAsync();

                Debug.WriteLine($"{DateTime.Now} Download trade history {startDate.Date} : {endDate.Date} ended\n");
                

                tempDays += 10;
            } while (startDate != StartDate);
            
            ViewBag.status = $"Download trade history {Pair} ended";
            th_list.Clear();

            return View();
        }

        private void CheckMinValue()
        {
            for (int i = 0; i < th_list.Count; i++)
            {
                if (th_list[i].Total < MinValue)
                {
                    th_list.RemoveAt(i);
                    i--;
                }
            }
            
        }

        private void CheckExist(string Pair, List<Coin_TH> DB)
        {
            for (int dbIndex = 0; dbIndex < DB.Count; dbIndex++)
            {
                if (DB[dbIndex].Type == Pair)
                {
                    for (int listIndex = 0; listIndex < th_list.Count; listIndex++)
                    {
                        if (DB[dbIndex].GlobalTradeId == th_list[listIndex].GlobalTradeId)
                        {
                            th_list.RemoveAt(listIndex);
                            listIndex--;
                        }
                    }
                }
            }
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
