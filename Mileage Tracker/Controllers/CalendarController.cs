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
    [VerifyLogin]
    public class CalendarController : Controller
    {

        private Calendar DB;

        public CalendarController()
        {
            this.DB = new Calendar();
        }

        // GET: Calendar
        public ActionResult Index()
        {
            ViewBag.users = DB.getActiveUsers();
            ViewBag.user = UserData.User.ID;
            return View();
        }
        public JsonResult GetEvents(int id = 0)
        {
            var events = DB.getAllDays();
            var userid = 0;
            if (id != 0)
            {
                events = DB.getUserDays(id);
                userid = events[0].User.ID;
            }
            var meets = DB.GetMeets();

            var runList = from e in events
                            select new
                            {
                                id = e.ID,
                                title = e.Distance + " - " + e.User.DisplayName,
                                start = e.Date.ToString("s"),
                                end = e.Date.ToString("s"),
                                allDay = true,
                                userId = e.User.ID,
                                type = "run"
                            };
            var meetList = from m in meets
                            select new
                            {
                                id = m.ID,
                                title = m.MeetName,
                                start = m.MeetDateStart.ToString("s"),
                                end = m.MeetDateEnd.ToString("s"),
                                allDay = true,
                                userId = id,
                                type = "meet"
                            };
            var eventList = runList.Union(meetList);
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}