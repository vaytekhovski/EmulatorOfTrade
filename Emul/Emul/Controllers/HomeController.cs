using System.Web.Mvc;

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