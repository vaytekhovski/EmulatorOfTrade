using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emul.Controllers.TestProg
{
    public class TestProgController : Controller
    {
        // GET: TestProg
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult TestStart()
        {
            return View();
        }
    }
}