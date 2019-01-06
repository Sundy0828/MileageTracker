using Mileage_Tracker.Classes;
using Mileage_Tracker.Models;
using Mileage_Tracker.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mileage_Tracker.DataLayer
{
    public class DataBase
    {
        protected RunningEntities DB;

        public DataBase()
        {
            this.DB = new RunningEntities();
        }

        public List<Role> getRoles()
        {
            return this.DB.Roles.ToList();
        }
        public List<RunType> getRunTypes()
        {
            return this.DB.RunTypes.ToList();
        }

        public List<User> getUsers()
        {
            return this.DB.Users.ToList();
        }
        public List<User> getActiveUsers()
        {
            return this.DB.Users.Where(u => u.Active == true).ToList();
        }
        public User getUser(String userName)
        {
            return this.DB.Users.Where(u => u.UserName == userName).FirstOrDefault();
        }
        public User getUser(int id)
        {
            return this.DB.Users.Where(u => u.ID == id).FirstOrDefault();
        }

        public List<RunningWeek> getWeeksByDate(DateTime date)
        {
            var days = this.DB.RunningCalendars.Where(c => c.Monday == date).OrderBy(c => c.User.ID).ThenBy(x => x.Date).ToList();
            List<RunningWeek> weeks = new List<RunningWeek>();
            User user = new User();
            var sum = 0.0;
            var runMiles = 0.0;
            var ctMiles = 0.0;
            foreach (var week in days)
            {

                if (user.ID == 0)
                {
                    user = week.User;
                    switch (week.RunType)
                    {
                        case 1:
                            runMiles += week.Distance;
                            break;
                        case 2:
                            ctMiles += week.Distance;
                            break;
                    }
                    sum += week.Distance;
                }
                else if (user != week.User)
                {
                    weeks.Add(new RunningWeek()
                    {
                        User = user,
                        Date = date,
                        totMiles = sum,
                        runMiles = runMiles,
                        ctMiles = ctMiles
                    });
                    user = week.User;
                    sum = week.Distance;
                    runMiles = 0.0;
                    ctMiles = 0.0;
                    switch (week.RunType)
                    {
                        case 1:
                            runMiles = week.Distance;
                            break;
                        case 2:
                            ctMiles = week.Distance;
                            break;
                    }
                }
                else
                {
                    switch (week.RunType)
                    {
                        case 1:
                            runMiles += week.Distance;
                            break;
                        case 2:
                            ctMiles += week.Distance;
                            break;
                    }
                    sum += week.Distance;
                }
            }
            if (sum > 0)
            {
                weeks.Add(new RunningWeek()
                {
                    User = user,
                    Date = date,
                    totMiles = sum,
                    runMiles = runMiles,
                    ctMiles = ctMiles
                });
            }
            return weeks;
        }

        public List<RunningWeek> getWeeks(int userId)
        {
            var days = this.DB.RunningCalendars.Where(c => c.UserID == userId).OrderBy(c => c.Monday).ThenBy(x => x.Date).ToList();
            List<RunningWeek> weeks = new List<RunningWeek>();
            DateTime day = new DateTime();
            var sum = 0.0;
            var runMiles = 0.0;
            var ctMiles = 0.0;
            foreach (var week in days)
            {
                
                if (day == new DateTime())
                {
                    day = week.Monday;

                    switch (week.RunType)
                    {
                        case 1:
                            runMiles += week.Distance;
                            break;
                        case 2:
                            ctMiles += week.Distance;
                            break;
                    }
                    sum += week.Distance;
                }
                else if (day != week.Monday)
                {
                    weeks.Add(new RunningWeek()
                    {
                        Date = day,
                        totMiles = sum,
                        runMiles = runMiles,
                        ctMiles = ctMiles
                    });
                    day = week.Monday;
                    sum = week.Distance;
                    runMiles = 0.0;
                    ctMiles = 0.0;
                    switch (week.RunType)
                    {
                        case 1:
                            runMiles = week.Distance;
                            break;
                        case 2:
                            ctMiles = week.Distance;
                            break;
                    }
                }
                else
                {
                    switch (week.RunType)
                    {
                        case 1:
                            runMiles += week.Distance;
                            break;
                        case 2:
                            ctMiles += week.Distance;
                            break;
                    }
                    sum += week.Distance;
                }
            }
            if (sum > 0)
            {
                weeks.Add(new RunningWeek()
                {
                    Date = day,
                    totMiles = sum,
                    runMiles = runMiles,
                    ctMiles = ctMiles
                });
            }
            return weeks;
        }
        public List<RunningCalendar> getAllDays()
        {
            return this.DB.RunningCalendars.ToList();
        }
        public List<RunningCalendar> getUserDays(int id)
        {
            return this.DB.RunningCalendars.Where(r => r.User.ID == id).ToList();
        }
        public List<RunningCalendar> getUserDaysByDate(int id, DateTime day)
        {
            return this.DB.RunningCalendars.Where(r => r.User.ID == id && r.Monday == day).ToList();
        }
        public RunningCalendar getWeek(int userId, DateTime day)
        {
            return this.DB.RunningCalendars.Where(w => w.UserID == userId && w.Date == day).FirstOrDefault();
        }

        public List<WeeklyPercnet> getPercents()
        {
            return this.DB.WeeklyPercnets.ToList();
        }
        public WeeklyPercnet getPercent(int id)
        {
            return this.DB.WeeklyPercnets.Where(p => p.ID == id).FirstOrDefault();
        }
        public List<Meet> GetMeets()
        {
            return DB.Meets.ToList();
        }
        public List<Meet> GetMeets(DateTime start, DateTime end)
        {
            return DB.Meets.Where(m => m.MeetDateStart >= start && m.MeetDateEnd <= end).ToList();
        }
        public Meet GetMeet(int id)
        {
            return DB.Meets.Where(m => m.ID == id).FirstOrDefault();
        }

        public Boolean ResetPassword(String email)
        {
            try
            {
                var password = CreatePassword(8);
                var newUser = getUser(email);
                newUser.Password = password;
                newUser.ResetNeeded = true;

                this.DB.Users.Add(newUser);
                this.DB.SaveChanges();

                Email.Mail(email, "Password", "Your new password is: " + password);

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

    }
}