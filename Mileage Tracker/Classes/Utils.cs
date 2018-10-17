using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}