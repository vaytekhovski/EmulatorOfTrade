using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;
using System.Diagnostics;
using Emulator.Models.DataBase.DBModels;

namespace Emulator.Controllers.Data
{
    public class DownloadController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }


        List<BTC_TH> btc_list = new List<BTC_TH>();
        List<XRP_TH> xrp_list = new List<XRP_TH>();
        List<ETH_TH> eth_list = new List<ETH_TH>();


        public ActionResult TradeHistoryDownload(DateTime StartDate, DateTime EndDate, string Pair)
        {

            DateTime start, end;
            start = EndDate;
            end = EndDate;

            int j = 0;
            do
            {
                end = EndDate.AddDays(-j);

                if (start.AddDays(-10).Date < StartDate)
                    start = StartDate;
                else
                    start = end.AddDays(-10);

                
                var lst = DownloadTradeHistory.CycleDownloadData(start, end, Pair);

                switch (Pair)
                {
                    case "BTC":
                        ConvertToTH(lst, "BTC");
                        foreach (var DBitem in OwnDataBase.database.BTC_TradeHistory)
                            for (int i = 0; i < btc_list.Count; i++)
                                if (DBitem.GlobalTradeId == btc_list[i].GlobalTradeId)
                                    btc_list.RemoveAt(i);

                        OwnDataBase.database.BTC_TradeHistory.AddRange(btc_list);
                        break;
                    case "XRP":
                        ConvertToTH(lst, "XRP");
                        foreach (var DBitem in OwnDataBase.database.XRP_TradeHistory)
                            for (int i = 0; i < xrp_list.Count; i++)
                                if (DBitem.GlobalTradeId == xrp_list[i].GlobalTradeId)
                                    xrp_list.RemoveAt(i);

                        OwnDataBase.database.XRP_TradeHistory.AddRange(xrp_list);
                        break;

                    case "ETH":
                        ConvertToTH(lst, "ETH");
                        foreach (var DBitem in OwnDataBase.database.ETH_TradeHistory)
                            for (int i = 0; i < eth_list.Count; i++)
                                if (DBitem.GlobalTradeId == eth_list[i].GlobalTradeId)
                                    eth_list.RemoveAt(i);

                        OwnDataBase.database.ETH_TradeHistory.AddRange(eth_list);
                        break;

                    default:
                        break;
                }

                
                OwnDataBase.database.SaveChanges();


                j += 10;
            } while (start != StartDate);
            
            ViewBag.status = $"Download trade history {Pair} ended";
            return View();
        }


        void ConvertToTH(List<TradeHistory> histories, string type)
        {
            switch (type)
            {
                case "BTC":
                    {
                        foreach(var item in histories)
                        {
                            item.Rate = item.Rate.Replace('.', ',');
                            item.Amount = item.Amount.Replace('.', ',');
                            item.Total = item.Total.Replace('.', ',');
                            btc_list.Add(new BTC_TH
                            {
                                GlobalTradeId = item.GlobalTradeId,
                                TradeId = item.TradeId,
                                Date = item.Date,
                                Type = item.Type.ToString(),
                                Rate = double.Parse(item.Rate),
                                Amount = double.Parse(item.Amount),
                                Total = double.Parse(item.Total)
                            });
                        }
                    }break;
                case "XRP":
                    {
                        foreach (var item in histories)
                        {
                            item.Rate = item.Rate.Replace('.', ',');
                            item.Amount = item.Amount.Replace('.', ',');
                            item.Total = item.Total.Replace('.', ',');
                            xrp_list.Add(new XRP_TH()
                            {
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
                    break;
                case "ETH":
                    {
                        foreach (var item in histories)
                        {
                            item.Rate = item.Rate.Replace('.', ',');
                            item.Amount = item.Amount.Replace('.', ',');
                            item.Total = item.Total.Replace('.', ',');
                            eth_list.Add(new ETH_TH()
                            {
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
                    break;
                default:
                    break;
            }
        }
    }
}