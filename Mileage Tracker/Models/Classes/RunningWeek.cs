using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mileage_Tracker.Models.Classes
{
    public class RunningWeek
    {
        public User User { get; set; }
        public DateTime Date { get; set; }
        public Double totMiles { get; set; }
    }
}