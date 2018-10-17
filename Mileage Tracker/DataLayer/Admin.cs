using Mileage_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mileage_Tracker.DataLayer
{
    public class Admin : DataBase
    {
        public Boolean CreateUser(String email, int utype, Boolean active)
        {
            try
            {
                var newUser = new User()
                {
                    UserName = email,
                    Password = CreatePassword(8),
                    UserLevel = utype,
                    ResetNeeded = true,
                    Active = active
                };

                this.DB.Users.Add(newUser);
                this.DB.SaveChanges();

                return true;
            }catch
            {
                return false;
            }
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}