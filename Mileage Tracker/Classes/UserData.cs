using Mileage_Tracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mileage_Tracker.Classes
{
    public static class UserData
    {
        public static User User
        {
            get
            {
                var user = new User();
                try
                {
                    var Json = HttpContext.Current.Request.Cookies["SHUXC"].Value;
                    user = JsonConvert.DeserializeObject<User>(Json);
                }
                catch
                {
                    user = new User
                    {
                        ID = -1,
                        UserName = "",
                        Password = "",
                        UserLevel = -1,
                        ResetNeeded = false,
                        Active = true,
                        DisplayName = "",
                        PeekMileage = 0,
                    };
                }
                return user;
            }
        }
    }
}