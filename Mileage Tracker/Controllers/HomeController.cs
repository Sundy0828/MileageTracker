using Mileage_Tracker.Classes;
using Mileage_Tracker.DataLayer;
using Mileage_Tracker.Models;
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

        [VerifyLogin]
        public ActionResult Index()
        {
            var plans = DB.GetWeeklyPlans();
            List<WeeklyPlan> weeklyPlans = new List<WeeklyPlan>();
            foreach (var plan in plans)
            {
                weeklyPlans.Add(new WeeklyPlan {
                    ID = plan.ID,
                    WeekOf = plan.WeekOf,
                    WeekPlan = HttpUtility.UrlEncode(plan.WeekPlan).Replace("+", " ")
                });
            }
            ViewBag.WeeklyPlans = weeklyPlans;
            return View();
        }
        public ActionResult Login()
        {
            if (Request.Cookies["SHUXC"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
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
                        Utils.setCookie(user);
                        if (user.ResetNeeded)
                        {
                            return RedirectToAction("Settings", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        
                    }
                }
            }

            return RedirectToAction("Login", "Home", new { successful = false });
        }
        // GET: Admin
        public ActionResult Logout()
        {
            Utils.deleteCookie();

            return RedirectToAction("Login", "Home");
        }

        [VerifyLogin]
        public ActionResult Settings()
        {
            return View();
        }
        [VerifyLogin]
        public ActionResult oldPlans()
        {
            var plans = DB.GetWeeklyPlans();
            List<WeeklyPlan> weeklyPlans = new List<WeeklyPlan>();
            foreach (var plan in plans)
            {
                weeklyPlans.Add(new WeeklyPlan
                {
                    ID = plan.ID,
                    WeekOf = plan.WeekOf,
                    WeekPlan = HttpUtility.UrlEncode(plan.WeekPlan).Replace("+", " ")
                });
            }
            ViewBag.WeeklyPlans = weeklyPlans;
            return View();
        }
        [VerifyLogin]
        public ActionResult oldPlan(int id)
        {
            var plan = DB.GetWeeklyPlan(id);
            List<WeeklyPlan> weeklyPlans = new List<WeeklyPlan>();

            weeklyPlans.Add(new WeeklyPlan
            {
                ID = plan.ID,
                WeekOf = plan.WeekOf,
                WeekPlan = HttpUtility.UrlEncode(plan.WeekPlan).Replace("+", " ")
            });
            
            ViewBag.WeeklyPlans = weeklyPlans[0];
            return View();
        }
        // GET: Admin
        public ActionResult UpdatePassword(String password, String cpassword)
        {
            if (password == cpassword)
            {
                if (DB.UpdatePassowrd(password))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Settings", "Home", new { successful = false });
        }

        [VerifyLogin]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [VerifyLogin]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}