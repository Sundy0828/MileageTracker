﻿using Mileage_Tracker.DataLayer;
using Mileage_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mileage_Tracker.Controllers
{
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
        public ActionResult CreateUser(String email, String dName, int utype, String active)
        {
            var user = DB.getUser(email);
            if (user == null)
            {
                DB.CreateUser(email, dName, utype, active == "on");
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("AddUser", "Admin");
        }
        // GET: Admin
        public ActionResult UpdateUser(String email, String dName, int utype, String active)
        {
            var user = DB.getUser(email);
            if (user != null)
            {
                DB.UpdateUser(email, dName, utype, active == "on");
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("AddUser", "Admin");
        }

    }
}