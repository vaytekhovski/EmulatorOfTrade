using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Emulator;

namespace Emulator.Controllers
{
    public class ChartDatasController : Controller
    {
        private Emulator db = new Emulator();

        // GET: ChartDatas
        public ActionResult Index()
        {
            return View(db.ChartDatas.ToList());
        }

        // GET: ChartDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChartData chartData = db.ChartDatas.Find(id);
            if (chartData == null)
            {
                return HttpNotFound();
            }
            return View(chartData);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
