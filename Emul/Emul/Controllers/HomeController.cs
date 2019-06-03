using System.Web.Mvc;

namespace TradeEmulatorMVC.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(string SortOrder)
        {
           
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return (RedirectToAction("../Account/Login"));
        }
        
        public ActionResult Main()
        {
            
            return View();
        }

    }
}