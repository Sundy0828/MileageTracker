using Mileage_Tracker.DataLayer;
using Mileage_Tracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mileage_Tracker.Classes
{
    public static class Utils
    {
        public static DateTime NextSunday(this DateTime from)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)DayOfWeek.Sunday;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
        public static DateTime StartOfWeek(this DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static String sha256(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public static void setCookie(User user)
        {
            HttpCookie myCookie = new HttpCookie("SHUXC");
            DateTime now = DateTime.Now;

            var curUser = new User
            {
                ID = user.ID,
                UserName = user.UserName,
                Password = user.Password,
                UserLevel = user.UserLevel,
                ResetNeeded = user.ResetNeeded,
                Active = user.Active,
                DisplayName = user.DisplayName,
                PeekMileage = user.PeekMileage,
            };

            // Set the cookie value.
            myCookie.Value = JsonConvert.SerializeObject(curUser);
            // Set the cookie expiration date.
            myCookie.Expires = now.AddYears(50); // For a cookie to effectively never expire

            // Add the cookie.
            HttpContext.Current.Response.SetCookie(myCookie);
        }
        public static void deleteCookie()
        {
            HttpCookie currentUserCookie = HttpContext.Current.Request.Cookies["SHUXC"];
            HttpContext.Current.Response.Cookies.Remove("SHUXC");
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            HttpContext.Current.Response.SetCookie(currentUserCookie);
        }
    }

    public class VerifyLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["SHUXC"];
            if (cookie == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}