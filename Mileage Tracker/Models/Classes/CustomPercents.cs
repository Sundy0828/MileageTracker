using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mileage_Tracker.Models.Classes
{
    public class CustomPercents
    {
        public String Name { get; set; }
        public DateTime Date { get; set; }
        public List<String> Percents { get; set; }
    }
}