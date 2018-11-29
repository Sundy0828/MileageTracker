using Mileage_Tracker.Classes;
using Mileage_Tracker.DataLayer;
using Mileage_Tracker.Models;
using Mileage_Tracker.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mileage_Tracker.Controllers
{
    [VerifyLogin]
    public class AthleteController : Controller
    {
        private Athlete DB;

        public AthleteController()
        {
            this.DB = new Athlete();
        }
        // GET: Athlete
        public ActionResult Index()
        {
            var weeks = DB.getWeeks(UserData.User.ID);
            ViewBag.weeks = weeks.OrderByDescending(w => w.Date);
            return View();
        }
        // GET: Athlete
        public ActionResult WhatToRun()
        {
            var user = DB.getUser(UserData.User.ID);

            var weeklyPercents = user.WeeklyPercnet.Percents;
            var weeklyPercent = new double[0];
            if (!String.IsNullOrEmpty(weeklyPercents))
            {
                weeklyPercent = weeklyPercents.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
            }

            List<DateTime> weeks = new List<DateTime>();
            var startDate = user.WeeklyPercnet.FirstWeek;
            for (int i = 0; i < weeklyPercent.Count(); i ++)
            {
                weeks.Add(Utils.StartOfWeek(startDate.AddDays(i*7)));
            }
            ViewBag.weeks = weeks;
            ViewBag.records = weeklyPercent.Count();
            ViewBag.percent = weeklyPercent;
            ViewBag.User = user;

            return View();
        }
        // GET: Athlete
        public ActionResult WeeklyMileage(int mileage)
        {
            ViewBag.mileage = mileage;
            return View();
        }

        // GET: Athlete
        public ActionResult Week(DateTime date)
        {
            var monday = Utils.StartOfWeek(date);
            List<RunningCalendar> days = new List<RunningCalendar>();

            var coachesNotes = "";

            for (var i = 0; i < 7; i++)
            {
                var dayOfWeek = monday.AddDays(i);
                var week = DB.getWeek(UserData.User.ID, dayOfWeek);
                if (week != null)
                {
                    days.Add(week);
                    if (!String.IsNullOrWhiteSpace(week.CoachNotes))
                    {
                        coachesNotes = week.CoachNotes;
                    }
                }
                else
                {
                    var day = new RunningCalendar()
                    {
                        UserID = UserData.User.ID,
                        Date = dayOfWeek,
                        Distance = 0,
                        RunType = 1,
                        Notes = ""
                    };
                    days.Add(day);
                }
            }

            ViewBag.CoachesNotes = coachesNotes;

            ViewBag.Monday = monday.Date;
            ViewBag.Days = days;
            ViewBag.runTypes = DB.getRunTypes();
            return View();
        }
        // GET: Athlete
        public ActionResult UpdateWeek(List<RunningDay> weeklyRuns)
        {
            var success = false;
            foreach (var run in weeklyRuns)
            {
                if (run.Distance > 0)
                {
                    var newRun = new RunningCalendar()
                    {
                        UserID = UserData.User.ID,
                        Date = run.Date,
                        Monday = run.Monday,
                        Distance = run.Distance,
                        RunType = run.RunType,
                        Notes = run.Notes

                    };
                    success = DB.UpdateDay(newRun);
                    if (!success)
                    {
                        return Json(new { success = false });
                    }
                }
            }
            return Json(new { success = true });
        }
    }
}