using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuickType;
namespace Emulator.Controllers
{
    public class TradeHistoriesController : Controller
    {
        private Emulator db = new Emulator();

        // GET: TradeHistories
        public ActionResult Index()
        {
            return View(db.TradeHistories.ToList());
        }

        // GET: TradeHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TradeHistory tradeHistory = db.TradeHistories.Find(id);
            if (tradeHistory == null)
            {
                return HttpNotFound();
            }
            return View(tradeHistory);
        }

        // GET: TradeHistories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TradeHistories/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.

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
