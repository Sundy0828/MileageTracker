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
            return View();
        }
        public JsonResult GetEvents()
        {
            
            var events = DB.getAllDays();

            var eventList = from e in events
                            select new
                            {
                                id = e.ID,
                                title = e.Distance + " - " + e.User.DisplayName,
                                start = e.Date.ToString("s"),
                                end = e.Date.ToString("s"),
                                allDay = true
                            };
        
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}