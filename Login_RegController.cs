using System.Web.Mvc;



namespace ERP_Web_MVC_Porject.Controllers
{
    public class Login_RegController : Controller
    {
        // GET: Login_Reg
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login_Reg
        public ActionResult Login()
        {
            return View();
        }
          

        // Post: Login_Reg
        [HttpPost ]     
        public ActionResult Login(FormCollection collection)
        {
            // Login_Service Log = new Login_Service();

            //  bool LoginAction = Log.Login(collection[1].ToString(), collection[1].ToString());

            //if (Log.Login(collection[1].ToString(), collection[1].ToString()) == true)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //else {
            //       return View();
            //     }

            return View();
        }




    }
}