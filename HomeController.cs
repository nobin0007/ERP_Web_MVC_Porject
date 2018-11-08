using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP_Web_MVC_Porject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            System.Web.HttpContext.Current.Session["LogUserName"] = "Nobin";
            return View();
        }

        public ActionResult Index_APT()
        {          
            return View();
        }

        public ActionResult Index_Acc()
        {
            return View();
        }

    }
}