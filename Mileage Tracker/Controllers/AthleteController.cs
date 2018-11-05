﻿using Mileage_Tracker.Classes;
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
            var weeks = DB.getWeeks(2);
            ViewBag.weeks = weeks;
            return View();
        }
        // GET: Athlete
        public ActionResult WhatToRun()
        {
            var weeklyPercent = new List<double> { 0.2, 0.3, 0.4,0.5,0.6,0.65,0.7,0.75,0.8,0.87,0.93,1,1,0.85,0.9,0.95,0.9,0.85,0.82,0.78,0.75,0.7,0.65,0.6,0.65,0.6,0.63,0.55 };
            List<DateTime> weeks = new List<DateTime>();
            var startDate = new DateTime(2018, 5, 21);
            for (int i = 0; i < weeklyPercent.Count(); i ++)
            {
                weeks.Add(Utils.StartOfWeek(startDate.AddDays(i*7)));
            }
            ViewBag.weeks = weeks;
            ViewBag.records = weeklyPercent.Count();
            ViewBag.percent = weeklyPercent;
            var user = DB.getUser(2);
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

            for (var i = 0; i < 7; i++)
            {
                var dayOfWeek = monday.AddDays(i);
                var week = DB.getWeek(2, dayOfWeek);
                if (week != null)
                {
                    days.Add(week);
                }
                else
                {
                    var day = new RunningCalendar()
                    {
                        UserID = 2,
                        Date = dayOfWeek,
                        Distance = 0,
                        RunType = 1,
                        Notes = ""
                    };
                    days.Add(day);
                }
            }

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
                        UserID = 2,
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