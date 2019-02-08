using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using Emulator.Models;

namespace Emulator
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
<<<<<<< HEAD
            Database.SetInitializer(new TradeHistoryDbInitializer());
            Database.SetInitializer(new ChartDataDbInitializer());
=======
            
>>>>>>> fddbc687aa235351b43b0af5ba7ed0577da3b641
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
