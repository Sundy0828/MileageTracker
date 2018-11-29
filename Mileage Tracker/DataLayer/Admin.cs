using Mileage_Tracker.Classes;
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
        public Boolean CreateUser(String email, String dName, int utype, int ptype, Boolean active, double pkMile)
        {
            try
            {
                var newUser = new User()
                {
                    UserName = email,
                    DisplayName = dName,
                    Password = Utils.sha256(CreatePassword(8)),
                    UserLevel = utype,
                    ResetNeeded = true,
                    Active = active,
                    Percents = ptype,
                    PeekMileage = pkMile
                };

                this.DB.Users.Add(newUser);
                this.DB.SaveChanges();

                return true;
            }catch
            {
                return false;
            }
        }
        public Boolean UpdateUser(String email, String dName, int utype, int ptype, Boolean active, double pkMile)
        {
            try
            {
                var newUser = getUser(email);
                newUser.UserName = email;
                newUser.DisplayName = dName;
                newUser.UserLevel = utype;
                newUser.Active = active;
                newUser.Percents = ptype;
                newUser.PeekMileage = pkMile;

                this.DB.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean UpdatePassowrd(String password)
        {
            try
            {
                var newUser = getUser(UserData.User.ID);
                newUser.Password = Utils.sha256(password);
                newUser.ResetNeeded = false;

                this.DB.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        private string CreatePassword(int length)
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

        public List<WeeklyPlan> GetWeeklyPlans()
        {
            return DB.WeeklyPlans.ToList();
        }
        public WeeklyPlan GetWeeklyPlan(int id)
        {
            return DB.WeeklyPlans.Where(w => w.ID == id).FirstOrDefault();
        }
        public Boolean CreatePlan(DateTime date, String plan)
        {
            try
            {
                var newPlan = new WeeklyPlan()
                {
                    WeekOf = date,
                    WeekPlan = plan
                };

                this.DB.WeeklyPlans.Add(newPlan);
                this.DB.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean UpdatePlan(int ID, DateTime date, String plan)
        {
            try
            {
                var newPlan = GetWeeklyPlan(ID);
                newPlan.WeekOf = date;
                newPlan.WeekPlan = plan;

                this.DB.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean AddPercents(WeeklyPercnet weeklyPercnet)
        {
            try
            {
                var newPercent = new WeeklyPercnet()
                {
                    FirstWeek = weeklyPercnet.FirstWeek,
                    Percents = weeklyPercnet.Percents,
                    Name = weeklyPercnet.Name
                };

                this.DB.WeeklyPercnets.Add(newPercent);
                this.DB.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean UpdatePercents(WeeklyPercnet weeklyPercnet)
        {
            try
            {
                var newPercent = getPercent(weeklyPercnet.ID);


                newPercent.FirstWeek = weeklyPercnet.FirstWeek;
                newPercent.Percents = weeklyPercnet.Percents;
                newPercent.Name = weeklyPercnet.Name;
                
                
                this.DB.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean UpdateDay(RunningCalendar day)
        {
            try
            {
                var newDay = day;
                var exists = getWeek(day.UserID, day.Date);
                if (exists == null)
                {
                    this.DB.RunningCalendars.Add(newDay);
                    this.DB.SaveChanges();
                }
                else
                {
                    exists.CoachNotes = newDay.CoachNotes;
                    this.DB.SaveChanges();

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}