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
        public ActionResult Index(string SortOrder)
        {

            return View();
        }
        
        public ActionResult Main()
        {
            
            return View();
        }

    }
}