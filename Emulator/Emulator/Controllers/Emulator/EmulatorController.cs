using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emulator.Controllers
{
    public class EmulatorController : Controller
    {
        // GET: Emulator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Emulation(string Pair, DateTime StartDate, DateTime EndDate, string diff, string checkTime, string buyTime, string holdTime)
        {
            Emulator.Models.Emulator.Emulator emulator = new Models.Emulator.Emulator(Pair, StartDate, EndDate, Double.Parse(diff), Double.Parse(checkTime), Double.Parse(buyTime), Double.Parse(holdTime));
            emulator.MakeMoney();
            return View();
        }
    }
}