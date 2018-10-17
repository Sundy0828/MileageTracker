using Mileage_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mileage_Tracker.DataLayer
{
    public class Athlete : DataBase
    {
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
                    exists.Distance = newDay.Distance;
                    exists.RunType = newDay.RunType;
                    exists.Notes = newDay.Notes;
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