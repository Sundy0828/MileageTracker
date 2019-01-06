using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mileage_Tracker.Models.Classes
{
    public class weekRunPlan
    {
        public DateTime dateStart { get; set; }
        public DateTime dateEnd { get; set; }
        public List<Meet> meets { get; set; }
        public double totMileage { get; set; }
        public double longRun { get; set; }
    }
}