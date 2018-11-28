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

        private Admin admin;
        private Calendar calendar;

        public AdminController()
        {
            this.admin = new Admin();
            this.calendar = new Calendar();
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
            ViewBag.Weeks = admin.getWeeksByDate(monday);
            ViewBag.Users = admin.getActiveUsers();
            return View();
        }

        // GET: Admin
        public ActionResult Week(DateTime date, int id)
        {
            ViewBag.user = admin.getUser(id);

            var monday = Utils.StartOfWeek(date);
            List<RunningCalendar> days = new List<RunningCalendar>();
            var runType = admin.getRunTypes();

            for (var i = 0; i < 7; i++)
            {
                var dayOfWeek = monday.AddDays(i);
                var week = admin.getWeek(id, dayOfWeek);
                if (week != null)
                {
                    days.Add(week);
                }
                else
                {
                    var day = new RunningCalendar()
                    {
                        UserID = id,
                        Date = dayOfWeek,
                        Distance = 0,
                        RunType = 1,
                        RunType1 = runType[0],
                        Notes = ""
                    };
                    days.Add(day);
                }
            }

            ViewBag.Monday = monday.Date;
            ViewBag.Days = days;
            ViewBag.runTypes = admin.getRunTypes();
            return View();
        }
        // GET: Admin
        public ActionResult UpdateWeek(DateTime Date, String Notes)
        {
            var success = false;
            var test = calendar.getUserDaysByDate(UserData.User.ID, Date);
            foreach (var run in test)
            {
                if (run.Distance > 0)
                {
                    var newRun = new RunningCalendar()
                    {
                        UserID = UserData.User.ID,
                        Date = run.Date,
                        CoachNotes = Notes

                    };
                    success = admin.UpdateDay(newRun);
                    if (!success)
                    {
                        return Json(new { success = false });
                    }
                }
            }
            return Json(new { success = true });
        }

        #region Weekly Percents
        // GET: Admin
        public ActionResult WeeklyPercent()
        {
            ViewBag.percents = admin.getPercents();
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
            var percent = admin.getPercent(id);
            var weeklyPercent = percent.Percents.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
            var monday = Utils.StartOfWeek(percent.FirstWeek);
            ViewBag.monday = monday;
            ViewBag.percents = weeklyPercent;
            ViewBag.name = percent.Name;
            return View();
        }
        public ActionResult AddPercents(String Name, String Date, String Percents)
        {
            var success = false;
            var newPercent = new WeeklyPercnet()
            {
                FirstWeek = DateTime.Parse(Date),
                Percents = Percents,
                Name = Name

            };
            success = admin.AddPercents(newPercent);
            if (!success)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
        public ActionResult UpdatePercents(int id, String Name, String Date, String Percents)
        {
            var success = false;
            var newPercent = new WeeklyPercnet()
            {
                ID = id,
                FirstWeek = DateTime.Parse(Date),
                Percents = Percents,
                Name = Name

            };
            success = admin.UpdatePercents(newPercent);
            if (!success)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }
        #endregion

        #region Weekly Plans
        // GET: Admin
        public ActionResult WeeklyPlan()
        {
            ViewBag.WeeklyPlans = admin.GetWeeklyPlans();
            return View();
        }
        // GET: Admin
        public ActionResult AddPlan()
        {
            var monday = Utils.StartOfWeek(DateTime.Now);
            ViewBag.Monday = monday.Date;
            ViewBag.WeeklyPlans = admin.GetWeeklyPlans();
            return View();
        }
        // GET: Admin
        public ActionResult EditPlan(int id)
        {
            var plan = admin.GetWeeklyPlan(id);
            ViewBag.Monday = plan.WeekOf.Date;
            ViewBag.plan = HttpUtility.UrlEncode(plan.WeekPlan).Replace("+", " ");
            return View();
        }
        // GET: Admin
        public ActionResult CreatePlan(DateTime Date, String Plan)
        {
            var plan = HttpUtility.UrlDecode(Plan);
            admin.CreatePlan(Date, plan);
            return RedirectToAction("WeeklyPlan", "Admin");
        }
        // GET: Admin
        public ActionResult UpdatePlan(int ID, DateTime Date, String Plan)
        {
            var plan = HttpUtility.UrlDecode(Plan);
            admin.UpdatePlan(ID, Date, plan);
            return RedirectToAction("WeeklyPlan", "Admin");
        }
        #endregion

        #region Users
        // GET: Admin
        public ActionResult Users()
        {
            ViewBag.Users = admin.getUsers();
            return View();
        }
        // GET: Admin
        public ActionResult AddUser()
        {
            var roles = admin.getRoles();
            ViewBag.roles = roles;
            ViewBag.percents = admin.getPercents();

            return View();
        }
        // GET: Admin
        public ActionResult EditUser(int id)
        {
            var roles = admin.getRoles();
            ViewBag.roles = roles;
            ViewBag.User = admin.getUser(id);
            ViewBag.percents = admin.getPercents();

            return View();
        }
        // GET: Admin
        public ActionResult CreateUser(String email, String dName, int utype, String active, double pkMile)
        {
            var user = admin.getUser(email);
            if (user == null)
            {
                admin.CreateUser(email, dName, utype, active == "on", pkMile);
                return RedirectToAction("Users", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        // GET: Admin
        public ActionResult UpdateUser(String email, String dName, int utype, String active, double pkMile)
        {
            var user = admin.getUser(email);
            if (user != null)
            {
                admin.UpdateUser(email, dName, utype, active == "on", pkMile);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Users", "Admin");
        }
        #endregion

    }
}