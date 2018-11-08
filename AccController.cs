using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP_Web_MVC_Porject.Controllers
{
    public class AccController : Controller
    {
        // GET: Acc
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChartOfAccount()
        {
            return View();
        }
    }
}