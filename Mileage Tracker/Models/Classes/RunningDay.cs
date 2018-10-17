using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mileage_Tracker.Models.Classes
{
    public class RunningDay
    {
        public DateTime Date { get; set; }
        public DateTime Sunday { get; set; }
        public double Distance { get; set; }
        public int RunType { get; set; }
        public String Notes { get; set; }
    }
}