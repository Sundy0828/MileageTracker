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
        public User getUser(String userName)
        {
            return this.DB.Users.Where(u => u.UserName == userName).FirstOrDefault();
        }

        public List<RunningWeek> getWeeks(int userId)
        {
            var days = this.DB.RunningCalendars.Where(c => c.UserID == userId).OrderBy(c => c.Sunday).ThenBy(x => x.Date).ToList();
            List<RunningWeek> weeks = new List<RunningWeek>();
            DateTime day = new DateTime();
            var sum = 0.0;
            foreach (var week in days)
            {
                
                if (day == new DateTime())
                {
                    day = week.Sunday;
                    sum += week.Distance;
                }
                else if (day != week.Sunday)
                {
                    weeks.Add(new RunningWeek()
                    {
                        Date = day,
                        totMiles = sum
                    });
                    day = week.Sunday;
                    sum = week.Distance;
                }
                else
                {
                    sum += week.Distance;
                }
            }
            weeks.Add(new RunningWeek()
            {
                Date = day,
                totMiles = sum
            });
            /*foreach (var week in days)
            {
                weeks.Add(new RunningWeek() {
                    Date = week.ToList();
                });
            }*/
            return weeks;
        }
        public RunningCalendar getWeek(int userId, DateTime day)
        {
            return this.DB.RunningCalendars.Where(w => w.UserID == userId && w.Date == day).FirstOrDefault();
        }

    }
}