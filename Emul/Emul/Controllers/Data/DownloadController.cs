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


        List<BTC_TH> btc_list = new List<BTC_TH>();
        List<XRP_TH> xrp_list = new List<XRP_TH>();
        List<ETH_TH> eth_list = new List<ETH_TH>();


        public ActionResult TradeHistoryDownload(DateTime StartDate, DateTime EndDate, string Pair)
        {

            DateTime start, end;
            start = EndDate;
            end = EndDate;
            List<BTC_TH> listBTC = new List<BTC_TH>();
            List<XRP_TH> listXRP = new List<XRP_TH>();
            List<ETH_TH> listETH = new List<ETH_TH>();
            switch (Pair)
            {
                case "BTC":
                    listBTC = OwnDataBase.database.BTC_TradeHistory.OrderBy(history => history.Date).ToList();
                    break;
                case "XRP":
                    listXRP = OwnDataBase.database.XRP_TradeHistory.OrderBy(history => history.Date).ToList();
                    break;
                case "ETH":
                    listETH = OwnDataBase.database.ETH_TradeHistory.OrderBy(history => history.Date).ToList();
                    break;
                default:
                    break;
            }
             
            int j = 0;
            do
            {
                end = EndDate.AddDays(-j);
                
                start = start.AddDays(-10).Date < StartDate ? StartDate : end.AddDays(-10);

                
                var lst = DownloadTradeHistory.CycleDownloadData(start, end, Pair);
                

                switch (Pair)
                {
                    case "BTC":
                        Debug.WriteLine($"Download trade history {start.Date} : {end.Date} started");
                        ConvertToTH(lst, "BTC");
                        for (int i = 0; i < listBTC.Count; i++)
                            for (int z = 0; z < btc_list.Count; z++)
                                if (listBTC[i].GlobalTradeId == btc_list[z].GlobalTradeId)
                                    btc_list.RemoveAt(z);

                        OwnDataBase.database.BTC_TradeHistory.AddRange(btc_list);
                        Debug.WriteLine($"Download trade history {start.Date} : {end.Date} ended");
                        break;
                    case "XRP":
                        Debug.WriteLine($"Download trade history {start.Date} : {end.Date} started");
                        ConvertToTH(lst, "XRP");
                        for (int i = 0; i < listXRP.Count; i++)
                            for (int z = 0; z < xrp_list.Count; z++)
                                if (listXRP[i].GlobalTradeId == xrp_list[z].GlobalTradeId)
                                    xrp_list.RemoveAt(z);

                        OwnDataBase.database.XRP_TradeHistory.AddRange(xrp_list);
                        Debug.WriteLine($"Download trade history {start.Date} : {end.Date} ended");
                        break;

                    case "ETH":
                        Debug.WriteLine($"Download trade history {start.Date} : {end.Date} started");
                        ConvertToTH(lst, "ETH");
                        for (int i = 0; i < listETH.Count; i++)
                            for (int z = 0; z < eth_list.Count; z++)
                                if (listETH[i].GlobalTradeId == eth_list[z].GlobalTradeId)
                                    eth_list.RemoveAt(z);
                        
                        OwnDataBase.database.ETH_TradeHistory.AddRange(eth_list);
                        Debug.WriteLine($"Download trade history {start.Date} : {end.Date} ended");
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
                        btc_list.Clear();
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
                        xrp_list.Clear();
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
                        eth_list.Clear();
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