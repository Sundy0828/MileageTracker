using Mileage_Tracker.Classes;
using Mileage_Tracker.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mileage_Tracker.Controllers
{
    public class HomeController : Controller
    {

        private Admin DB;

        public HomeController()
        {
            this.DB = new Admin();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        // GET: Admin
        public ActionResult UserLogin(String email, String password, String rememberMe)
        {
            if (!String.IsNullOrWhiteSpace(email))
            {
                var userName = email;
                if (!email.Contains("@setonhill.edu"))
                {
                    userName = email + "@setonhill.edu";
                }
                var user = DB.getUser(userName);
                if (user != null)
                {
                    if (user.Password == Utils.sha256(password))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}