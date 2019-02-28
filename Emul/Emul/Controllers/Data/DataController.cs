using System;
using System.Collections.Generic;
using System.Web.Mvc;
using QuickType;
using Emulator.Models;
using System.Data.Entity;

namespace Emulator.Controllers
{
    public class DataController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }
        
    }
}