using Mileage_Tracker.Classes;
using Mileage_Tracker.DataLayer;
using Mileage_Tracker.Models;
using Mileage_Tracker.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mileage_Tracker.Controllers
{
    [VerifyLogin]
    public class AdminController : Controller
    {

        private Admin DB;

        public AdminController()
        {
            this.DB = new Admin();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin
        public ActionResult Reports(DateTime date)
        {
            var monday = Utils.StartOfWeek(date);
            ViewBag.monday = monday;
            ViewBag.Weeks = DB.getWeeksByDate(monday);
            ViewBag.Users = DB.getActiveUsers();
            return View();
        }
        // GET: Admin
        public ActionResult WeeklyPercent()
        {
            ViewBag.percents = DB.getPercents();
            return View();
        }
        // GET: Admin
        public ActionResult AddWeeklyPercent()
        {
            var monday = Utils.StartOfWeek(DateTime.Now);
            ViewBag.monday = monday;
            return View();
        }
        // GET: Admin
        public ActionResult EditWeeklyPercent(int id)
        {
            var percent = DB.getPercent(id);
            var weeklyPercent = percent.Percents.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
            var monday = Utils.StartOfWeek(percent.FirstWeek);
            ViewBag.monday = monday;
            ViewBag.percents = weeklyPercent;
            return View();
        }

        #region Weekly Plans
        // GET: Admin
        public ActionResult WeeklyPlan()
        {
            ViewBag.WeeklyPlans = DB.GetWeeklyPlans();
            return View();
        }
        // GET: Admin
        public ActionResult AddPlan()
        {
            var monday = Utils.StartOfWeek(DateTime.Now);
            ViewBag.Monday = monday.Date;
            ViewBag.WeeklyPlans = DB.GetWeeklyPlans();
            return View();
        }
        // GET: Admin
        public ActionResult EditPlan(int id)
        {
            var plan = DB.GetWeeklyPlan(id);
            ViewBag.Monday = plan.WeekOf.Date;
            ViewBag.plan = HttpUtility.UrlEncode(plan.WeekPlan).Replace("+", " ");
            return View();
        }
        // GET: Admin
        public ActionResult CreatePlan(DateTime Date, String Plan)
        {
            var plan = HttpUtility.UrlDecode(Plan);
            DB.CreatePlan(Date, plan);
            return RedirectToAction("WeeklyPlan", "Admin");
        }
        // GET: Admin
        public ActionResult UpdatePlan(int ID, DateTime Date, String Plan)
        {
            var plan = HttpUtility.UrlDecode(Plan);
            DB.UpdatePlan(ID, Date, plan);
            return RedirectToAction("WeeklyPlan", "Admin");
        }
        #endregion

        #region Users
        // GET: Admin
        public ActionResult Users()
        {
            ViewBag.Users = DB.getUsers();
            return View();
        }
        // GET: Admin
        public ActionResult AddUser()
        {
            var roles = DB.getRoles();
            ViewBag.roles = roles;

            return View();
        }
        // GET: Admin
        public ActionResult EditUser(int id)
        {
            var roles = DB.getRoles();
            ViewBag.roles = roles;
            ViewBag.User = DB.getUser(id);

            return View();
        }
        // GET: Admin
        public ActionResult CreateUser(String email, String dName, int utype, String active, double pkMile)
        {
            var user = DB.getUser(email);
            if (user == null)
            {
                DB.CreateUser(email, dName, utype, active == "on", pkMile);
                return RedirectToAction("Users", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        // GET: Admin
        public ActionResult UpdateUser(String email, String dName, int utype, String active, double pkMile)
        {
            var user = DB.getUser(email);
            if (user != null)
            {
                DB.UpdateUser(email, dName, utype, active == "on", pkMile);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Users", "Admin");
        }
        #endregion

    }
}